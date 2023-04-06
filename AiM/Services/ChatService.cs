using System;
using System.Collections.ObjectModel;
using AiM.Models;
using OpenAI_API;
using OpenAI_API.Chat;

namespace AiM.Services
{
    public class ChatService
    {
        private OpenAIAPI api;
        private Conversation chat;

        public ObservableCollection<ChatData> ConversationData { get; set; }

        public ChatService(Agent agent)
        {
            api = new OpenAIAPI(Preferences.Default.Get("OPENAI_API_KEY", ""));
            chat = api.Chat.CreateConversation();
            chat.Model = OpenAI_API.Models.Model.ChatGPTTurbo;
            ConversationData = new ObservableCollection<ChatData>();
            chat.AppendSystemMessage(agent.SystemMessage);
            if (agent.Examples != null)
            {
                foreach (var example in agent.Examples)
                {
                    chat.AppendUserInput(example.UserInput);
                    chat.AppendExampleChatbotOutput(example.ChatbotOutput);
                }
            }
        }

        public async Task Send(string message)
        {
            ConversationData.Add(new ChatData("Me", message));
            chat.AppendUserInput(message);
            var response = await chat.GetResponseFromChatbotAsync();
            ConversationData.Add(new ChatData("AiM", response.Trim('\n')));
        }
    }
}

