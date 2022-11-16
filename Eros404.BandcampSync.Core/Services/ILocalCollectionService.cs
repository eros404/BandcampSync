using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services
{
    public interface ILocalCollectionService
    {
        string CollectionPath { get; }

        Collection GetLocalCollection(bool asAlbums);
        void AddAlbum(Stream stream, Album album);
    }
}