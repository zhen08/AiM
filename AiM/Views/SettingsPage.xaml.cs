using AiM.Data;
using AiM.Models;

namespace AiM.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(AiMDatabase database)
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        OpenAiApiKeyEntry.Text = Settings.OpenAiApiKey;
        AzureCVEPEntry.Text = Settings.AzureCVEndPoint;
        AzureCVKeyEntry.Text = Settings.AzureCVApiKey;
        AzureBSEPEntry.Text = Settings.AzureBingSearchEndPoint;
        AzureBSKeyEntry.Text = Settings.AzureBingSearchApiKey;
        AzureDBEPEntry.Text = Settings.AzureCosmosDbEndPoint;
        AzureDBKeyEntry.Text = Settings.AzureCosmosDbApiKey;
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        Settings.OpenAiApiKey = OpenAiApiKeyEntry.Text;
        Settings.AzureCVEndPoint = AzureCVEPEntry.Text;
        Settings.AzureCVApiKey = AzureCVKeyEntry.Text;
        Settings.AzureBingSearchEndPoint = AzureBSEPEntry.Text;
        Settings.AzureBingSearchApiKey = AzureBSKeyEntry.Text;
        Settings.AzureCosmosDbEndPoint = AzureDBEPEntry.Text;
        Settings.AzureCosmosDbApiKey = AzureDBKeyEntry.Text;
    }
}
