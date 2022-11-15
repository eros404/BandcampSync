namespace Eros404.BandcampSync.Core.Models
{
    public class MissingTrack: Track, IComparable<MissingTrack>
    {
        public MissingTrack() { }
        public MissingTrack(Track track)
        {
            Title = track.Title;
            BandName = track.BandName;
            AlbumTitle = track.AlbumTitle;
            Number = track.Number;
            RedownloadUrl = track.RedownloadUrl;
        }

        public int CompareTo(MissingTrack? other)
        {
            return base.CompareTo(other);
        }
    }
}
