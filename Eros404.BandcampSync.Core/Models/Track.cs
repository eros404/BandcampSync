namespace Eros404.BandcampSync.Core.Models
{
    public class Track : CollectionItem, IEquatable<Track>, IComparable<Track>
    {
        public uint Number { get; init; }
        public string? AlbumTitle { get; init; }

        public int CompareTo(Track? other)
        {
            if (other == null)
                return 1;
            var bandNameCompare = string.Compare(BandName ?? "", other.BandName, StringComparison.Ordinal);
            if (bandNameCompare != 0)
                return bandNameCompare;
            var albumTitleCompare = string.Compare(AlbumTitle ?? "", other.AlbumTitle, StringComparison.Ordinal);
            return albumTitleCompare != 0 ? albumTitleCompare : Number.CompareTo(other.Number);
        }

        public bool Equals(Track? other)
        {
            if (other == null)
                return false;
            return Title == other.Title &&
                BandName == other.BandName &&
                Number == other.Number &&
                AlbumTitle == other.AlbumTitle;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Track);
        }

        public override int GetHashCode()
        {
            return (Title, BandName, Number, AlbumTitle).GetHashCode();
        }

        public override string ToString()
        {
            return $"{Title} - {BandName}";
        }
    }
}
