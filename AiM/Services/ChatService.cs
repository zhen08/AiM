using System;
using System.Collections.ObjectModel;
using AiM.Models;
using OpenAI_API;
using OpenAI_API.Chat;

namespace AiM.Services
{
    public class ChatService
    {
        const string OpenAI_API_Key = "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";


        private OpenAIAPI api;
        private Conversation chat;

        public ObservableCollection<ChatData> ConversationData { get; private set; }

        public ChatService(string systemMessage)
        {
            api = new OpenAIAPI(OpenAI_API_Key);
            chat = api.Chat.CreateConversation();
            chat.Model = OpenAI_API.Models.Model.ChatGPTTurbo;
            ConversationData = new ObservableCollection<ChatData>();
            if (String.IsNullOrEmpty(systemMessage))
            {
//                chat.AppendSystemMessage("You are ChatGPT, a large language model trained by OpenAI. Answer as concisely as possible. Knowledge cutoff: {knowledge_cutoff} Current date: {current_date}\n");
            } else
            {
                chat.AppendSystemMessage(systemMessage);
            }
        }

        public async Task Send(string message)
        {
            ConversationData.Add(new ChatData("Me", message));
            chat.AppendUserInput(message);
            var response = await chat.GetResponseFromChatbot();
            ConversationData.Add(new ChatData("OpenAI", response.Trim('\n')));
        }
    }
}

