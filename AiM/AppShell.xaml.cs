using AiM.Views;

namespace AiM;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(NewsPage), typeof(NewsPage));
        Routing.RegisterRoute(nameof(WebPage), typeof(WebPage));
    }
}

