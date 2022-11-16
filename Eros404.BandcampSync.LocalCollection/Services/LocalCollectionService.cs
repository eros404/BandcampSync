using System.IO.Compression;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.LocalCollection.Extensions;

namespace Eros404.BandcampSync.LocalCollection.Services
{
    public class LocalCollectionService : ILocalCollectionService
    {
        private readonly string _collectionPath;

        public LocalCollectionService(IWritableOptions<LocalCollectionOptions> options)
        {
            _collectionPath = options.Value.Path;
        }

        public string CollectionPath => _collectionPath;

        public Collection GetLocalCollection(bool asAlbums)
        {
            var allFiles = Directory.GetFiles(_collectionPath, "*.*", SearchOption.AllDirectories)
                .Where(filePath => new[]
                {
                    ".flac",
                    ".mp3",
                    ".wav",
                    ".aac",
                    ".ogg",
                    ".aiff"
                }.Contains(Path.GetExtension(filePath)))
                .Select(TagLib.File.Create);

            return asAlbums ? new Collection
            {
                Albums = allFiles.ToAlbums()
            } : new Collection
            {
                Tracks = allFiles.ToTracks()
            };
        }

        private static string GetFileNameFriendly(string content) => Path.GetInvalidFileNameChars()
            .Aggregate(content, (current, c) => current.Replace(c.ToString(), string.Empty));

        public void AddAlbum(Stream stream, Album album)
        {
            using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
            var directory = Path.Combine(_collectionPath,
                $"{GetFileNameFriendly(album.BandName)} - {GetFileNameFriendly(album.Title)}");
            Directory.CreateDirectory(directory);
            zip.ExtractToDirectory(directory);
        }
    }
}
