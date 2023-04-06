using System;
using AiM.Models;
using SQLite;

namespace AiM.Data
{
    public class AiMDatabase
    {

        SQLiteAsyncConnection Database;

        public async Task<List<Agent>> GetAgentsAsync()
        {
            await Init();
            var agents = await Database.Table<Agent>().ToListAsync();
            foreach (var agent in agents)
            {
                agent.Examples = (await Database.Table<ExampleChat>().Where(c => c.AgentId == agent.Id).ToListAsync());
            }
            return agents;
        }

        public async Task ResetAgentsAsync()
        {
            await Init();
            await Database.DeleteAllAsync<ExampleChat>();
            await Database.DeleteAllAsync<Agent>();
            await InsertDefaultAgents();
        }

        async Task InsertDefaultAgents()
        {
            await Database.InsertAsync(new Agent
            {
                Description = "General Chat",
                SystemMessage = "You are ChatGPT, a large language model trained by OpenAI. Answer as concisely as possible."
            });
            await Database.InsertAsync(new Agent
            {
                Description = "Proofreader",
                SystemMessage = "I want you act as a proofreader. I will provide you texts in any language and I would like you to detect the language, review them for any spelling, grammar, or punctuation errors. Once you have finished reviewing the text, provide me with any necessary corrections or suggestions for improve the text in the same langeuage as in the given texts."
            });
            await Database.InsertAsync(new Agent
            {
                Description = "Wikipedia",
                SystemMessage = "I want you to act as a Wikipedia page. I will give you the name of a topic, and you will provide a summary of that topic in the format of a Wikipedia page. Your summary should be informative and factual, covering the most important aspects of the topic. Start your summary with an introductory paragraph that gives an overview of the topic."
            });
            await Database.InsertAsync(new Agent
            {
                Description = "Travel Guide",
                SystemMessage = "I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location."
            });
            await Database.InsertAsync(new Agent
            {
                Description = "English Translator",
                SystemMessage = "I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations."
            });
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            await Database.CreateTableAsync<ExampleChat>();
            var result = await Database.CreateTableAsync<Agent>();
            if (result == CreateTableResult.Created)
            {
                await InsertDefaultAgents();
            }
        }
    }



    public static class Constants
    {
        public const string DatabaseFilename = "AiM.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}

