namespace Eros404.BandcampSync.BandcampApi.Models
{
    internal class CollectionItemResponse
    {
        public long item_id { get; set; }
        public string? item_type { get; set; }
        public long sale_item_id { get; set; }
        public string? item_title { get; set; }
        public long album_id { get; set; }
        public string? album_title { get; set; }
        public string? band_name { get; set; }
        public uint num_streamable_tracks { get; set; }
        public uint featured_track_number { get; set; }
    }
}
