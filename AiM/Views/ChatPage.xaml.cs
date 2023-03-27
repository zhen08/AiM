using AiM.Services;

namespace AiM.Views;

public partial class ChatPage : ContentPage
{
    private ChatService chatService;
    public ChatPage()
    {
        InitializeComponent();
        chatService = new ChatService("");
        BindingContext = chatService;
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
}
