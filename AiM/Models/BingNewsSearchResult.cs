using System;
namespace AiM.Models
{
    public class BingNewsSearchResult
    {
        public string _type { get; set; }
    }
    public class Image
    {
        public Thumbnail thumbnail { get; set; }
    }

    public class Provider
    {
        public string _type { get; set; }
        public string name { get; set; }
        public Image image { get; set; }
    }

    public class QueryContext
    {
        public string originalQuery { get; set; }
        public bool adultIntent { get; set; }
    }

    public class Sort
    {
        public string name { get; set; }
        public string id { get; set; }
        public bool isSelected { get; set; }
        public string url { get; set; }
    }

    public class Thumbnail
    {
        public string contentUrl { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Value
    {
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public List<Provider> provider { get; set; }
        public DateTime datePublished { get; set; }
        public string category { get; set; }
        public Image image { get; set; }
    }

    public class BingNewsSearchNews : BingNewsSearchResult
    {
        public string readLink { get; set; }
        public QueryContext queryContext { get; set; }
        public int totalEstimatedMatches { get; set; }
        public List<Sort> sort { get; set; }
        public List<Value> value { get; set; }
    }

    public class BingNewsSearchError : BingNewsSearchResult
    {
        public string code { get; set; }
        public string message { get; set; }
        public string moreDetails { get; set; }
        public string parameter { get; set; }
        public string subCode { get; set; }
        public string value { get; set; }
    }
}

