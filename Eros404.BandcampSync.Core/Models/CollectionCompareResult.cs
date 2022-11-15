namespace Eros404.BandcampSync.Core.Models
{
    public class CollectionCompareResult
    {
        public List<MissingAlbum> MissingAlbums { get; set; } = new();
        public List<MissingTrack> MissingTracks { get; set; } = new();

        public void SortItems()
        {
            MissingAlbums.Sort();
            MissingTracks.Sort();
        }
    }
}
