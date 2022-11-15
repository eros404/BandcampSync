namespace Eros404.BandcampSync.Core.Models
{
    public class MissingAlbum : Album, IComparable<MissingAlbum>
    {
        public uint NumberOfMissingTracks { get; set; }

        public MissingAlbum() { }
        public MissingAlbum(Album album, uint numberOfMissingTracks)
        {
            Title = album.Title;
            BandName = album.BandName;
            NumberOfTracks = album.NumberOfTracks;
            NumberOfMissingTracks = numberOfMissingTracks;
        }

        public int CompareTo(MissingAlbum? other)
        {
            return base.CompareTo(other);
        }
    }
}
