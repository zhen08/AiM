using System;
namespace AiM.Data
{
	public class Settings
	{
		public string OpenAiApiKey
		{
			get => Preferences.Default.Get("OPENAI_API_KEY", "");
			set => Preferences.Default.Set("OPENAI_API_KEY", value);
		}

        public string AzureCVEndPoint
        {
            get => Preferences.Default.Get("AZURE_CV_EP", "");
            set => Preferences.Default.Set("AZURE_CV_EP", value);
        }

        public string AzureCVApiKey
        {
            get => Preferences.Default.Get("AZURE_CV_KEY", "");
            set => Preferences.Default.Set("AZURE_CV_KEY", value);
        }
    }
}

