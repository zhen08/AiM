using System.Net;
using System.Text;
using AiM.Data;
using AiM.Models;
using AiM.Services;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Maui.Graphics.Platform;

namespace AiM.Views;

[QueryProperty(nameof(ChatAgent), "ChatAgent")]
public partial class ChatPage : ContentPage
{

    Agent chatAgent;
    Settings _settings;

    public Agent ChatAgent
    {
        get => chatAgent;
        set => chatAgent = value;
    }

    private ChatService _chatService;
    public ChatPage(Settings settings, ChatService chatService)
    {
        InitializeComponent();
        _settings = settings;
        _chatService = chatService;
        BindingContext = _chatService;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Title = chatAgent.Description;
        base.OnNavigatedTo(args);
        _chatService.StartConversation(ChatAgent);
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        _chatService.FinishConversation();
    }

    void PromptEditor_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        {
            InputGrid.TranslateTo(0, -400, 50);
        }
    }

    void PromptEditor_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        {
            InputGrid.TranslateTo(0, 0, 50);
        }
    }

    async void ChatCollectionView_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not ChatData chatData)
            return;
        await Clipboard.Default.SetTextAsync(chatData.Message);
        await DisplayAlert("Information", "Message text copied to clipboard.", "OK");
    }

    async void CameraBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                using (var sourceStream = await photo.OpenReadAsync())
                {
                    var image = PlatformImage.FromStream(sourceStream);
                    if (image != null)
                    {
                        using (var newImage = image.Downsize(1024, true))
                        {
                            var visionCredentials = new ApiKeyServiceClientCredentials(_settings.AzureCVApiKey);
                            var client = new ComputerVisionClient(visionCredentials);
                            client.Endpoint = _settings.AzureCVEndPoint;
                            var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, newImage.AsStream(), OcrLanguages.En);
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
                            PromptEditor.Text = resultBuilder.ToString();
                        }
                    }
                }
            }
        }
    }

    string previousText = "";

    async void PromptEditor_Completed(System.Object sender, System.EventArgs e)
    {
        var currentText = PromptEditor.Text;
        if ((!String.IsNullOrWhiteSpace(currentText)) && (!previousText.Equals(currentText)))
        {
            previousText = currentText;
            PromptEditor.Text = "";
            PromptEditor.IsEnabled = false;
            await _chatService.Send(currentText);
            PromptEditor.IsEnabled = true;
        }
    }
}
