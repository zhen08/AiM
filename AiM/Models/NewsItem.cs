using System;
namespace AiM.Models
{
    public class NewsItem
    {
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime FetchDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Abstract { get; set; }
    }
}

