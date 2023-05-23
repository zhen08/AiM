using System;
using AiM.Models;
using Microsoft.Azure.Cosmos;
using SQLite;

namespace AiM.Data
{
    public class AiMDatabase
    {

        SQLiteAsyncConnection Database;

        public async Task<List<ChatPrompt>> GetAgentsAsync()
        {
            await Init();
            var agents = await Database.Table<ChatPrompt>().ToListAsync();
            return agents;
        }

        public async Task UpdateAgentsAsync()
        {
            await Init();
            await Database.DeleteAllAsync<ChatPrompt>();
            using CosmosClient client = new(
                accountEndpoint: Settings.AzureCosmosDbEndPoint,
                authKeyOrResourceToken: Settings.AzureCosmosDbApiKey);
            var database = client.GetDatabase("AiM");
            var container = database.GetContainer("ChatPrompts");
            using FeedIterator<ChatPrompt> feed = container.GetItemQueryIterator<ChatPrompt>( queryText: "SELECT * FROM ChatPrompts");

            while (feed.HasMoreResults)
            {
                FeedResponse<ChatPrompt> chatPrompts = await feed.ReadNextAsync();

                foreach (var prompt in chatPrompts)
                {
                    await Database.InsertAsync(prompt);
                }
            }
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            var result = await Database.CreateTableAsync<ChatPrompt>();
            if (result == CreateTableResult.Created)
            {
                await UpdateAgentsAsync();
            }
        }

    }

}

