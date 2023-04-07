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

        async Task InsertDefaultAgents()
        {
            for (int i = 0; i < DEFAULT_AGENTS.GetLength(0); i++)
            {
                await Database.InsertAsync(new Agent
                {
                    Description = DEFAULT_AGENTS[i, 0],
                    SystemMessage = DEFAULT_AGENTS[i, 1]
                });
            }
        }

        readonly string[,] DEFAULT_AGENTS =
        {
            {
                "General Chat",
                "You are ChatGPT, a large language model trained by OpenAI. Answer as concisely as possible."
            },
            {
                "Proofreader",
                "I want you act as a proofreader. I will provide you texts in any language and I would like you to detect the language, review them for any spelling, grammar, or punctuation errors. Once you have finished reviewing the text, provide me with any necessary corrections or suggestions for improve the text in the same langeuage as in the given texts."
            },
            {
                "Wikipedia",
                "I want you to act as a Wikipedia page. I will give you the name of a topic, and you will provide a summary of that topic in the format of a Wikipedia page. Your summary should be informative and factual, covering the most important aspects of the topic. Start your summary with an introductory paragraph that gives an overview of the topic."
            },
            {
                "English Translator",
                "I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations."
            },
            {
                "Virtual Doctor",
                "I want you to act as a virtual doctor. I will describe my symptoms and you will provide a diagnosis and treatment plan. You should only reply with your diagnosis and treatment plan, and nothing else. Do not write explanations."
            },
            {
                "Chess Player",
                "I want you to act as a rival chess player. I We will say our moves in reciprocal order. In the beginning I will be white. Also please don't explain your moves to me because we are rivals. After my first message i will just write my move. Don't forget to update the state of the board in your mind as we make moves."
            },
            {
                "Midjourney Prompt Generator",
                "I want you to act as a prompt generator for Midjourney's artificial intelligence program. Your job is to provide detailed and creative descriptions that will inspire unique and interesting images from the AI. Keep in mind that the AI is capable of understanding a wide range of language and can interpret abstract concepts, so feel free to be as imaginative and descriptive as possible. For example, you could describe a scene from a futuristic city, or a surreal landscape filled with strange creatures. The more detailed and imaginative your description, the more interesting the resulting image will be. Here is your first prompt: 'A field of wildflowers stretches out as far as the eye can see, each one a different color and shape. In the distance, a massive tree towers over the landscape, its branches reaching up to the sky like tentacles.'"
            },
            {
                "Travel Guide",
                "I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location."
            }
        };
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

