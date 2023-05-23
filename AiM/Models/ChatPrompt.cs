using System;
using SQLite;

namespace AiM.Models
{
	public class ChatPrompt
	{
        [PrimaryKey]
        public string id { get; set; }
		public string prompt { get; set; }
	}
}

