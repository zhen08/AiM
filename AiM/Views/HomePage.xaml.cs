using System.Collections.ObjectModel;
using AiM.Data;
using AiM.Models;
using AiM.ViewModels;

namespace AiM.Views;

public partial class HomePage : ContentPage
{
    AiMDatabase _database;

    public HomePage(AiMDatabase database)
    {
        InitializeComponent();
        _database = database;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (((HomePageViewModel)this.BindingContext).Agents.Count == 0)
        {
            await ((HomePageViewModel)this.BindingContext).LoadItems(_database);
        }
    }
}
