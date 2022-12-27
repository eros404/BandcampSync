using System.Diagnostics.CodeAnalysis;

namespace Eros404.BandcampSync.BandcampApi.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CollectionSummaryResponse : ErrorResponse
    {
        public int fan_id { get; set; }
    }
}
