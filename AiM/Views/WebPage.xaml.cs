using HtmlAgilityPack;

namespace AiM.Views;

public partial class WebPage : ContentPage, IQueryAttributable
{
    string _url;

	public WebPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Url"))
        {
            _url = query["Url"] as string;
            PageWebView.Source = _url;
        }
        if (query.ContainsKey("Title"))
        {
            Title = query["Title"] as string;
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        PageWebView.Source = new HtmlWebViewSource
        {
            Html = @"<HTML><BODY></BODY></HTML>"
        };
        base.OnNavigatedFrom(args);
    }
    async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            Uri uri = new Uri(_url);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.External);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }

        //var web = new HtmlWeb();
        //var doc = await web.LoadFromWebAsync(_url);

        //string text = doc.DocumentNode.InnerText;
        //await Clipboard.SetTextAsync(text);
        //await DisplayAlert("Information", "Webpage text copied to clipboard.", "OK");
    }
}
