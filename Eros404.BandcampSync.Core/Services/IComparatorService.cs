using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services;

public interface IComparatorService
{
    Task<CollectionCompareResult?> CompareLocalWithBandcamp(string? search = null, bool ignorePhantoms = false);
}