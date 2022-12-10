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
        public BandcampApiService(IOptions<BandcampOptions> options, IUserSettingsService userSettingsService)
        {
            var baseUri = new Uri(options.Value.BaseUrl);
            _cookieContainer.Add(baseUri,
                new Cookie("identity", userSettingsService.GetValue(UserSettings.BandcampIdentityCookie), "/",
                    baseUri.Host));
            _client = new HttpClient(new HttpClientHandler { CookieContainer = _cookieContainer })
            {
                BaseAddress = new Uri(baseUri, "api/")
            };
            _getItemsCount = options.Value.GetItemsCount;
        }

        public async Task<Collection?> GetCollectionAsync()
        {
            var fanId = await GetFanIdAsync();
            return fanId == null ? null : await GetCollectionAsync((int)fanId);
        }

        private async Task<int?> GetFanIdAsync()
        {
            var response = await _client.GetAsync("fan/2/collection_summary");
            var collectionSummary = await response.EnsureSuccessAndReadFromJsonAsync<CollectionSummaryResponse>();
            return collectionSummary?.fan_id;
        }

        private async Task<Collection?> GetCollectionAsync(int fanId)
        {
            CollectionResponse? lastResponse = null;
            Collection? collection = null;
            do
            {
                lastResponse = lastResponse == null
                    ? await FetchItems(fanId, $"{DateTime.UtcNow.GetTimeStamp()}::a::")
                    : await FetchItems(fanId, lastResponse.last_token!);
                if (collection == null)
                {
                    collection = lastResponse?.ToCollection();
                }
                else if (lastResponse != null)
                {
                    collection.AddDistinct(lastResponse.ToCollection());
                }
                Console.WriteLine("pass");
            } while (lastResponse is { more_available: true } && !string.IsNullOrEmpty(lastResponse.last_token));
            return collection;
        }
        
        private async Task<CollectionResponse?> FetchItems(int fanId, string olderThanToken)
        {
            var response = await _client.PostAsJsonAsync("fancollection/1/collection_items", new
            {
                fan_id = fanId,
                older_than_token = olderThanToken,
                count = _getItemsCount
            });
            return await response.EnsureSuccessAndReadFromJsonAsync<CollectionResponse>();
        }
    }
}
