using AiM.Data;
using AiM.Models;

namespace AiM.Views;

public partial class SettingsPage : ContentPage
{

    AiMDatabase _database;
    Settings _settings;

    public SettingsPage(AiMDatabase database, Settings settings)
    {
        InitializeComponent();
        _database = database;
        _settings = settings;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        OpenAiApiKeyEntry.Text = _settings.OpenAiApiKey;
        AzureCVEPEntry.Text = _settings.AzureCVEndPoint;
        AzureCVKeyEntry.Text = _settings.AzureCVApiKey;
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        _settings.OpenAiApiKey = OpenAiApiKeyEntry.Text;
        _settings.AzureCVEndPoint = AzureCVEPEntry.Text;
        _settings.AzureCVApiKey = AzureCVKeyEntry.Text;
    }

    async void ResetBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        await _database.ResetAgentsAsync();
        await Shell.Current.GoToAsync("..", true, new Dictionary<string, object> { ["Refresh"] = true });
    }
}
