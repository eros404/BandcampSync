using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.BandcampApi.Models
{
    internal class CollectionResponse
    {
        public List<CollectionItemResponse> items { get; set; } = new();
        public Dictionary<string, string> redownload_urls { get; set; } = new();
        public Collection ToCollection()
        {
            var collection = new Collection();
            items.ForEach(item =>
            {
                switch (item.item_type)
                {
                    case "album":
                        collection.Albums.Add(new()
                        {
                            Title = item.item_title,
                            NumberOfTracks = item.num_streamable_tracks,
                            BandName = item.band_name
                        });
                        break;
                    case "track":
                        collection.Tracks.Add(new()
                        {
                            Number = item.featured_track_number,
                            Title = item.item_title,
                            AlbumTitle = item.album_title,
                            BandName = item.band_name
                        });
                        break;
                    default:
                        break;
                }
            });
            return collection;
        }
    }
}
