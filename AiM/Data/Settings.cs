using System;
namespace AiM.Data
{
	public static class Settings
	{
		public static string OpenAiApiKey
		{
			get => Preferences.Default.Get("OPENAI_API_KEY", "");
			set => Preferences.Default.Set("OPENAI_API_KEY", value);
		}

        public static string AzureCVEndPoint
        {
            get => Preferences.Default.Get("AZURE_CV_EP", "");
            set => Preferences.Default.Set("AZURE_CV_EP", value);
        }

        public static string AzureCVApiKey
        {
            get => Preferences.Default.Get("AZURE_CV_KEY", "");
            set => Preferences.Default.Set("AZURE_CV_KEY", value);
        }

        public static string AzureBingSearchEndPoint
        {
            get => Preferences.Default.Get("AZURE_BS_EP", "");
            set => Preferences.Default.Set("AZURE_BS_EP", value);
        }

        public static string AzureBingSearchApiKey
        {
            get => Preferences.Default.Get("AZURE_BS_KEY", "");
            set => Preferences.Default.Set("AZURE_BS_KEY", value);
        }

        public static string AzureCosmosDbEndPoint
        {
            get => Preferences.Default.Get("AZURE_DB_EP", "");
            set => Preferences.Default.Set("AZURE_DB_EP", value);
        }

        public static string AzureCosmosDbApiKey
        {
            get => Preferences.Default.Get("AZURE_DB_KEY", "");
            set => Preferences.Default.Set("AZURE_DB_KEY", value);
        }
    }
}

