using System;
namespace AiM.Models
{
    public class ChatData
    {
        public string Sender { get; private set; }
        public string Message { get; private set; }

        public ChatData(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public void AppendMessage(string message)
        {
            Message = Message + message;
        }
    }
}

