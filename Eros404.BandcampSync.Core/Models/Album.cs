namespace Eros404.BandcampSync.Core.Models
{
    public class Album : CollectionItem, IComparable<Album>
    {
        public uint NumberOfTracks { get; set; }

        public int CompareTo(Album? other)
        {
            if (other == null)
                return 1;
            var bandNameCompare = string.Compare((BandName ?? ""), other.BandName, StringComparison.Ordinal);
            return bandNameCompare != 0
                ? bandNameCompare
                : string.Compare((Title ?? ""), other.Title, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"{Title} - {BandName}";
        }
    }
}
