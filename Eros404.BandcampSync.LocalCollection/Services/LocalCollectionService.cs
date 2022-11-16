using System.IO.Compression;
using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Extensions;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.LocalCollection.Extensions;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.LocalCollection.Services
{
    public class LocalCollectionService : ILocalCollectionService
    {
        private readonly string _collectionPath;

        public LocalCollectionService(IOptions<LocalCollectionOptions> options)
        {
            _collectionPath = options.Value.Path;
        }

        public string CollectionPath => _collectionPath;

        public Collection GetLocalCollection(bool asAlbums)
        {
            var audioExtensions = (from object? audioFormat in Enum.GetValues(typeof(AudioFormat))
                select ((AudioFormat)audioFormat).GetExtension()).ToList();
            var allFiles = Directory.GetFiles(_collectionPath, "*.*", SearchOption.AllDirectories)
                .Where(filePath => audioExtensions.Contains(Path.GetExtension(filePath)))
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
                $"{GetFileNameFriendly(album.BandName ?? "")} - {GetFileNameFriendly(album.Title ?? "")}");
            Directory.CreateDirectory(directory);
            zip.ExtractToDirectory(directory);
        }

        public void AddTrack(Stream stream, Track track, AudioFormat audioFormat)
        {
            var filePath = Path.Combine(_collectionPath,
                $"{GetFileNameFriendly(track.BandName ?? "")} - {GetFileNameFriendly(track.Title ?? "")}{audioFormat.GetExtension()}");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            stream.CopyTo(fileStream);
        }
    }
}
