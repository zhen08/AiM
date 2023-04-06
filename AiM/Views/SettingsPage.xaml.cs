using AiM.Data;
using AiM.Models;

namespace AiM.Views;

public partial class SettingsPage : ContentPage
{

    AiMDatabase database;
    public SettingsPage(AiMDatabase aiMDatabase)
    {
        InitializeComponent();
        ApiKeyEntry.Text = Preferences.Default.Get("OPENAI_API_KEY", "");
        database = aiMDatabase;
    }

    void ApiKeyEntry_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        Preferences.Default.Set("OPENAI_API_KEY", ApiKeyEntry.Text);
    }

    async void ResetBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        await database.ResetAgentsAsync();
    }
}
