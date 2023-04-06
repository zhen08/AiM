using System.Collections.ObjectModel;
using AiM.Data;
using AiM.Models;

namespace AiM.Views;

public partial class HomePage : ContentPage
{
    AiMDatabase database;
    public ObservableCollection<Agent> ChatAgents { get; set; } = new();

    public HomePage(AiMDatabase aiMDatabase)
    {
        InitializeComponent();
        database = aiMDatabase;
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (ChatAgents.Count == 0)
        {
            var agents = await database.GetAgentsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var agent in agents)
                {
                    ChatAgents.Add(agent);
                }
            });
        }
    }

    async void SettingsBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    async void AgentsCollectionView_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Agent agent)
            return;
        await Shell.Current.GoToAsync(nameof(ChatPage), true, new Dictionary<string, object>
        {
            ["ChatAgent"] = agent
        }) ;
    }
}
