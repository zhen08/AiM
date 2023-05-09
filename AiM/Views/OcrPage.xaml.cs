using System.Text;
using AiM.Data;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Maui.Graphics.Platform;

namespace AiM.Views;

public partial class OcrPage : ContentPage
{
    string _prompt;
    Settings _settings;

    public OcrPage(Settings settings)
    {
        InitializeComponent();
        _settings = settings;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

        if (photo != null)
        {
            using (var sourceStream = await photo.OpenReadAsync())
            {
                using (var image = PlatformImage.FromStream(sourceStream))
                {
                    if (image != null)
                    {
                        using (var newImage = image.Downsize(1024, true))
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                var memStream = new MemoryStream();
                                newImage.AsStream().CopyTo(memStream);
                                OcrImage.Source = ImageSource.FromStream(() => memStream);
                            });
                            var visionCredentials = new ApiKeyServiceClientCredentials(_settings.AzureCVApiKey);
                            var client = new ComputerVisionClient(visionCredentials);
                            client.Endpoint = _settings.AzureCVEndPoint;
                            RunningIndicator.IsVisible = true;
                            try
                            {
                                var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, newImage.AsStream());
                                StringBuilder resultBuilder = new StringBuilder();
                                foreach (var region in ocrResult.Regions)
                                {
                                    foreach (var line in region.Lines)
                                    {
                                        foreach (var word in line.Words)
                                        {
                                            resultBuilder.Append(word.Text);
                                            resultBuilder.Append(' ');
                                        }
                                    }
                                }
                                _prompt = resultBuilder.ToString();
                            }
                            catch (Exception ex)
                            {
                                _prompt = ex.ToString();
                            }
                        }

                        RunningIndicator.IsVisible = false;
                    }
                }
            }
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }

    async void DoneButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true, new Dictionary<string, object> { ["Prompt"] = _prompt });
    }
}
