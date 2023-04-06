using SQLite;

namespace AiM.Models
{
    public class Agent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public string Description { get; set; }
        public string SystemMessage { get; set; }
        [Ignore]
        public List<ExampleChat> Examples { get; set; }
    }

    public class ExampleChat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int AgentId { get; set; }
        public string UserInput { get; set; }
        public string ChatbotOutput { get; set; }
    }
}

