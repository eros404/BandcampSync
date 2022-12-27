using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;

namespace Eros404.BandcampSync.Comparator.Services;

public class ComparatorService : IComparatorService
{
    private readonly IBandcampApiService _bandCampService;
    private readonly ILocalCollectionService _localCollectionService;
    private readonly IPhantomService _phantomService;

    public ComparatorService(IBandcampApiService bandCampService, ILocalCollectionService localCollectionService,
        IPhantomService phantomService)
    {
        _bandCampService = bandCampService;
        _localCollectionService = localCollectionService;
        _phantomService = phantomService;
    }

    public async Task<CollectionCompareResult?> CompareLocalWithBandcamp(bool ignorePhantoms = false)
    {
        var bandcamp = await _bandCampService.GetCollectionAsync();
        if (bandcamp == null)
            return null;
        if (!ignorePhantoms) bandcamp.Remove(_phantomService.GetPhantoms());
        var local = _localCollectionService.GetLocalCollection(false);
        return bandcamp.Compare(local.Tracks);
    }
}