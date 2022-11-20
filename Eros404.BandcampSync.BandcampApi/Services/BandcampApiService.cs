using Eros404.BandcampSync.Core.Models;
using System.Net.Http.Json;
using Eros404.BandcampSync.BandcampApi.Models;
using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.AppSettings.Models;
using System.Net;
using Eros404.BandcampSync.BandcampApi.Extensions;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.BandcampApi.Services
{
    public class BandcampApiService : IBandcampApiService
    {
        private readonly CookieContainer _cookieContainer = new();
        private readonly HttpClient _client;
        private readonly int _getItemsCount;
        public BandcampApiService(IOptions<BandcampOptions> options)
        {
            var baseUri = new Uri(options.Value.BaseUrl);
            _cookieContainer.Add(baseUri,
                new Cookie("identity", options.Value.IdentityCookie, "/", baseUri.Host));
            _client = new HttpClient(new HttpClientHandler { CookieContainer = _cookieContainer })
            {
                BaseAddress = new Uri(baseUri, "api/")
            };
            _getItemsCount = options.Value.GetItemsCount;
        }

        public async Task<int?> GetFanIdAsync()
        {
            var response = await _client.GetAsync("fan/2/collection_summary");
            var collectionSummary = await response.EnsureSuccessAndReadFromJsonAsync<CollectionSummaryResponse>();
            return collectionSummary?.fan_id;
        }

        public async Task<Collection?> GetCollectionAsync(int fanId)
        {
            var response = await _client.PostAsJsonAsync("fancollection/1/collection_items", new
            {
                fan_id = fanId,
                older_than_token = $"{GetNowTimeStamp()}::a::",
                count = _getItemsCount
            });
            var content = await response.EnsureSuccessAndReadFromJsonAsync<CollectionResponse>();
            return content?.ToCollection();

            static int GetNowTimeStamp() =>
                (int) DateTime.UtcNow.AddDays(1).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
