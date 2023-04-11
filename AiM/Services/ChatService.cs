using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using AiM.Data;
using AiM.Models;
using OpenAI_API;
using OpenAI_API.Chat;

namespace AiM.Services
{
    public class ChatService
    {
        private OpenAIAPI _api;
        private Conversation _chat;
        public ObservableCollection<ChatData> ConversationData { get; set; }

        public ChatService(IHttpClientFactory httpClientFactory, Settings settings)
        {
            ConversationData = new ObservableCollection<ChatData>();
            _api = new OpenAIAPI(settings.OpenAiApiKey);
            _api.HttpClientFactory = httpClientFactory;
        }

        public void StartConversation(Agent agent)
        {
            _chat = _api.Chat.CreateConversation();
            _chat.Model = OpenAI_API.Models.Model.ChatGPTTurbo;
            _chat.AppendSystemMessage(agent.SystemMessage);
            if (agent.Examples != null)
            {
                foreach (var example in agent.Examples)
                {
                    _chat.AppendUserInput(example.UserInput);
                    _chat.AppendExampleChatbotOutput(example.ChatbotOutput);
                }
            }
        }

        public void FinishConversation()
        {
            ConversationData.Clear();
            _chat = null;
        }

        public async Task Send(string message)
        {
            ConversationData.Add(new ChatData("Me", message));
            _chat.AppendUserInput(message);
            var response = await _chat.GetResponseFromChatbotAsync();
            ConversationData.Add(new ChatData("AiM", response.Trim('\n')));
        }
    }
}

