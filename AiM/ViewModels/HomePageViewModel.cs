using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AiM.Data;
using AiM.Models;
using AiM.Views;

namespace AiM.ViewModels
{
    public class HomePageViewModel : BindableObject
    {
        ObservableCollection<ChatPrompt> _agents;
        public ObservableCollection<ChatPrompt> Agents
        {
            get { return _agents; }
            set
            {
                _agents = value;
                OnPropertyChanged();
            }
        }

        public HomePageViewModel()
        {
            _agents = new ObservableCollection<ChatPrompt>();
        }

        public async Task LoadItems(AiMDatabase database)
        {
            var agents = await database.GetAgentsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _agents.Clear();
                foreach (var agent in agents)
                {
                    _agents.Add(agent);
                }
            });
        }

        public ICommand SelectCommand => new Command<object>(OnSelect);

        async void OnSelect(object obj)
        {
            if (obj is not ChatPrompt agent)
                return;
            await Shell.Current.GoToAsync(nameof(ChatPage), true, new Dictionary<string, object>
            {
                ["ChatAgent"] = agent
            });
        }

        public ICommand SettingsCommand => new Command<object>(OnSettings);

        async void OnSettings(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
    }
}

