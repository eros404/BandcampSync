using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services;

public interface IPhantomService
{
    Collection GetPhantoms();
    void AddPhantoms(params CollectionItem[] items);
    void RemovePhantoms(params CollectionItem[] items);
}