namespace Eros404.BandcampSync.Core.Models
{
    public class Album : CollectionItem, IEquatable<Album>, IComparable<Album>
    {
        public uint NumberOfTracks { get; set; }

        public int CompareTo(Album? other)
        {
            if (other == null)
                return 1;
            var bandNameCompare = string.Compare(BandName ?? "", other.BandName, StringComparison.Ordinal);
            return bandNameCompare != 0
                ? bandNameCompare
                : string.Compare(Title ?? "", other.Title, StringComparison.Ordinal);
        }

        public bool Equals(Album? other)
        {
            if (other == null)
                return false;
            return Title == other.Title &&
                   BandName == other.BandName &&
                   NumberOfTracks == other.NumberOfTracks;
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Album);
        }
        public override int GetHashCode()
        {
            return (Title, BandName, NumberOfTracks).GetHashCode();
        }

        public override string ToString()
        {
            return $"{Title} - {BandName}";
        }
    }
}
