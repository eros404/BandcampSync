namespace Eros404.BandcampSync.Core.Models
{
    public class Collection
    {
        public List<Album> Albums { get; set; } = new();
        public List<Track> Tracks { get; set; } = new();

        public void AddDistinct(Collection collection)
        {
            Albums.AddRange(collection.Albums);
            Tracks.AddRange(collection.Tracks);
            Albums = Albums.Distinct().ToList();
            Tracks = Tracks.Distinct().ToList();
        }

        public void SortItems()
        {
            Albums.Sort();
            Tracks.Sort();
        }

        public List<CollectionItem> GetAllItems()
        {
            var items = new List<CollectionItem>();
            items.AddRange(Albums);
            items.AddRange(Tracks);
            return items;
        }

        public CollectionCompareResult Compare(IEnumerable<Track> tracks)
        {
            var missingAlbums = new List<MissingAlbum>();
            Albums.ForEach(album =>
            {
                var numberOfTracksFromThisAlbum = tracks.Count(track => track.AlbumTitle == album.Title);
                if (album.NumberOfTracks != numberOfTracksFromThisAlbum && album.NumberOfTracks > numberOfTracksFromThisAlbum)
                    missingAlbums.Add(new MissingAlbum(album, (uint)(album.NumberOfTracks - numberOfTracksFromThisAlbum)));
            });
            return new CollectionCompareResult
            {
                MissingTracks = Tracks.Except(tracks)
                    .Select(track => new MissingTrack(track))
                    .ToList(),
                MissingAlbums = missingAlbums
            };
        }
    }
}
