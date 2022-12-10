using Eros404.BandcampSync.BandcampApi.Extensions;
using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.BandcampApi.Models
{
    internal class CollectionResponse : ErrorResponse
    {
        public List<CollectionItemResponse> items { get; set; } = new();
        public Dictionary<string, string> redownload_urls { get; set; } = new();
        public bool more_available { get; set; }
        public string? last_token { get; set; }
        public Collection ToCollection()
        {
            var redownloadUrls = redownload_urls.ToDictionary(keyValue => keyValue.Key.KeepOnlyNumericCharacters(),
                keyValue => keyValue.Value);
            var collection = new Collection();
            items.ForEach(item =>
            {
                switch (item.item_type)
                {
                    case "album":
                        collection.Albums.Add(new Album
                        {
                            Title = item.item_title,
                            NumberOfTracks = item.num_streamable_tracks,
                            BandName = item.band_name,
                            RedownloadUrl = redownloadUrls[item.sale_item_id.ToString()]
                        });
                        break;
                    case "track":
                        collection.Tracks.Add(new Track
                        {
                            Number = item.featured_track_number ?? 0,
                            Title = item.item_title,
                            AlbumTitle = item.album_title,
                            BandName = item.band_name,
                            RedownloadUrl = redownloadUrls[item.sale_item_id.ToString()]
                        });
                        break;
                }
            });
            return collection;
        }
    }
}
