using AiM.Services;
using AiM.ViewModels;

namespace AiM.Views;

public partial class NewsPage : ContentPage
{

    BingNewsSearchService _bingNewsSearchService;

    public NewsPage(BingNewsSearchService bingNewsSearchService)
    {
        InitializeComponent();
        _bingNewsSearchService = bingNewsSearchService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((NewsPageViewModel)this.BindingContext).FetchNews(_bingNewsSearchService);
    }
}
