using AiM.Models;
using AiM.Services;

namespace AiM.Views;

[QueryProperty(nameof(ChatAgent), "ChatAgent")]
public partial class ChatPage : ContentPage
{

    Agent chatAgent;

    public Agent ChatAgent
    {
        get => chatAgent;
        set => chatAgent = value;
    }

    private ChatService chatService;
    public ChatPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        chatService = new ChatService(ChatAgent);
        BindingContext = chatService;
        Title = chatAgent.Description;
    }


    async void SendBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        SendBtn.IsEnabled = false;
        await chatService.Send(PromptEditor.Text);
        PromptEditor.Text = "";
        SendBtn.IsEnabled = true;
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
}
