using System.Net;
using System.Text;
using AiM.Data;
using AiM.Models;
using AiM.Services;
using Microsoft.Maui.Graphics.Platform;

namespace AiM.Views;

public partial class ChatPage : ContentPage, IQueryAttributable
{

    ChatPrompt _chatAgent;
    string _prompt;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ChatAgent"))
        {
            _chatAgent = query["ChatAgent"] as ChatPrompt;
        }
        if (query.ContainsKey("Prompt"))
        {
            _prompt = query["Prompt"] as string;
            query.Remove("Prompt");
        }
    }

    private ChatService _chatService;
    public ChatPage(ChatService chatService)
    {
        InitializeComponent();
        _chatService = chatService;
        BindingContext = _chatService;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Title = _chatAgent.id;
        _chatService.StartConversation(_chatAgent);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        _chatService.FinishConversation();
        _chatAgent = null;
        _prompt = null;
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
        ChatCollectionView.SelectedItem = null;
        await Clipboard.Default.SetTextAsync(chatData.Message);
        await DisplayAlert("Information", "Message text copied to clipboard.", "OK");
    }

    async void SendBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        await SendChat();
    }

    string previousText = "";

    async void PromptEditor_Completed(System.Object sender, System.EventArgs e)
    {
        await SendChat();
    }

    async Task SendChat()
    {
        var currentText = PromptEditor.Text;
        if ((!String.IsNullOrWhiteSpace(currentText)) && (!previousText.Equals(currentText)))
        {
            RunningIndicator.IsRunning = true;
            previousText = currentText;
            PromptEditor.Text = "";
            PromptEditor.IsEnabled = false;
            DeviceDisplay.Current.KeepScreenOn = true;
            await _chatService.Send(currentText);
            DeviceDisplay.Current.KeepScreenOn = false;
            PromptEditor.IsEnabled = true;
            RunningIndicator.IsRunning = false;
        }
    }
}
