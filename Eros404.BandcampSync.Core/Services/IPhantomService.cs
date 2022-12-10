using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services;

public interface IPhantomService
{
    Collection GetPhantoms();
    void AddPhantom(CollectionItem item);
    void RemovePhantom(CollectionItem item);
}