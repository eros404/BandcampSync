namespace Eros404.BandcampSync.Core.Models
{
    public class Collection
    {
        public List<Album> Albums { get; set; } = new();
        public List<Track> Tracks { get; set; } = new();

        public void SortItems()
        {
            Albums.Sort();
            Tracks.Sort();
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
