using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AiM.Models;
using AiM.Services;
using AiM.Views;

namespace AiM.ViewModels
{

    public class NewsPageViewModel : BindableObject
    {
        DateTime _lastFetchTime = DateTime.MinValue;

        ObservableCollection<NewsItem> _news = new();
        public ObservableCollection<NewsItem> News
        {
            get { return _news; }
            set
            {
                _news = value;
                OnPropertyChanged();
            }
        }

        public NewsPageViewModel()
        {
        }

        public async Task FetchNews(BingNewsSearchService bingNewsSearchService)
        {
            if ((DateTime.Now - _lastFetchTime).TotalHours > 1)
            {
                var oldNews = _news.Where(n => (DateTime.Now - n.FetchDate).TotalHours > 24).ToList();
                foreach (var news in oldNews)
                {
                    _news.Remove(news);
                }
                await LoadNews(bingNewsSearchService, "建筑机器人", BingNewsSearchMarket.ZH_CN);
                await LoadNews(bingNewsSearchService, "智能建造", BingNewsSearchMarket.ZH_CN);
                await LoadNews(bingNewsSearchService, "职业教育", BingNewsSearchMarket.ZH_CN);
                await LoadNews(bingNewsSearchService, "Construction Robot", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "Robotics", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "Navigation", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "Autonomous Driving", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "机器人", BingNewsSearchMarket.ZH_CN);
                await LoadNews(bingNewsSearchService, "ChatGPT", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "ChatGPT", BingNewsSearchMarket.ZH_CN);
                await LoadNews(bingNewsSearchService, "Bard", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "Large Language Model", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "AGI", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "Stable Diffusion", BingNewsSearchMarket.EN_US);
                await LoadNews(bingNewsSearchService, "Midjourney", BingNewsSearchMarket.EN_US);
                _lastFetchTime = DateTime.Now;
            }
        }

        async Task LoadNews(BingNewsSearchService bingNewsSearchService, string query, BingNewsSearchMarket bingNewsSearchMarket, int count = 10)
        {
            try
            {
                var newsResult = await bingNewsSearchService.SearchNewsAsync(query, bingNewsSearchMarket, count);
                foreach (var newsValue in newsResult.value.Where(n => (DateTime.Now - n.datePublished).TotalHours < 72).OrderByDescending(v => v.datePublished))
                {
                    if (!_news.Any(n => n.Title.Equals(newsValue.name)))
                    {
                        _news.Add(new NewsItem
                        {
                            PublishDate = newsValue.datePublished,
                            FetchDate = DateTime.Now,
                            Title = newsValue.name,
                            Description = newsValue.description,
                            Url = newsValue.url,
                            ThumbnailUrl = newsValue.image != null ? newsValue.image.thumbnail.contentUrl : "news.png"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public ICommand SelectCommand => new Command<object>(OnSelect);

        async void OnSelect(object obj)
        {
            if (obj is not NewsItem newsItem)
                return;
            await Shell.Current.GoToAsync(nameof(WebPage), true, new Dictionary<string, object>
            {
                ["Url"] = newsItem.Url,
                ["Title"] = newsItem.Title
            });
        }
    }
}

