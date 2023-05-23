using System;
using AiM.Data;
using AiM.Models;
using Newtonsoft.Json;

namespace AiM.Services
{
    public enum BingNewsSearchMarket
    {
        EN_US,
        EN_GB,
        EN_AU,
        EN_CA,
        ZH_CN,
        ZH_TW,
        ZH_HK
    }

    public class BingNewsSearchService
    {
        const string QUERY_PARAMETER = "v7.0/news/search?q=";  // Required
        const string MKT_PARAMETER = "&mkt=";
        const string COUNT_PARAMETER = "&count=";
        const string OFFSET_PARAMETER = "&offset=";
        const string ORIGINAL_IMG_PARAMETER = "&originalImg=";
        const string SAFE_SEARCH_PARAMETER = "&safeSearch=";
        const string SORT_BY_PARAMETER = "&sortBy=";
        const string TEXT_DECORATIONS_PARAMETER = "&textDecorations=";
        const string TEXT_FORMAT_PARAMETER = "&textFormat=";
        const string FRESHNESS_PARAMETER = "&freshness=";

        string _clientIdHeader = null;
        HttpClient _client;
        string _baseUri;

        public BingNewsSearchService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Settings.AzureBingSearchApiKey);
            _baseUri = Settings.AzureBingSearchEndPoint;
        }

        public async Task<BingNewsSearchNews> SearchNewsAsync(string searchString, BingNewsSearchMarket market = BingNewsSearchMarket.EN_US, int count = 30)
        {
            var queryString = QUERY_PARAMETER + Uri.EscapeDataString(searchString);
            queryString += MKT_PARAMETER + market.ToString().Replace('_', '-');
            queryString += FRESHNESS_PARAMETER + "Day";
            queryString += COUNT_PARAMETER + count.ToString();

            HttpResponseMessage response = await _client.GetAsync(_baseUri + queryString);

            _clientIdHeader = response.Headers.GetValues("X-MSEdge-ClientID").FirstOrDefault();

            var contentString = await response.Content.ReadAsStringAsync();

            var baseResult = JsonConvert.DeserializeObject<BingNewsSearchResult>(contentString);
            switch (baseResult._type)
            {
                case "News":
                    return JsonConvert.DeserializeObject<BingNewsSearchNews>(contentString);
                case "Error":
                    var error = JsonConvert.DeserializeObject<BingNewsSearchError>(contentString);
                    throw new Exception("Error: " + error.message);
                default: throw new Exception("Unknown Result: " + contentString);
            }
        }
    }
}

