﻿using System.IO.Compression;
using Eros404.BandcampSync.Core.Extensions;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Eros404.BandcampSync.LocalCollection.Extensions;

namespace Eros404.BandcampSync.LocalCollection.Services
{
    public class LocalCollectionService : ILocalCollectionService
    {
        public LocalCollectionService(string collectionPath)
        {
            CollectionPath = collectionPath;
        }

        public string CollectionPath { get; }

        public Collection GetLocalCollection(bool asAlbums)
        {
            var audioExtensions = (from object? audioFormat in Enum.GetValues(typeof(AudioFormat))
                select ((AudioFormat)audioFormat).GetExtension()).ToList();
            var allFiles = Directory.GetFiles(CollectionPath, "*.*", SearchOption.AllDirectories)
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
            .Aggregate(content, (current, c) => current.Replace(c.ToString(), "-"));

        private void AddAlbum(Stream stream, Album album)
        {
            using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
            var directory = Path.Combine(CollectionPath,
                $"{GetFileNameFriendly(album.BandName ?? "")} - {GetFileNameFriendly(album.Title ?? "")}");
            Directory.CreateDirectory(directory);
            zip.ExtractToDirectory(directory);
        }

        private void AddTrack(Stream stream, Track track, AudioFormat audioFormat)
        {
            var filePath = Path.Combine(CollectionPath,
                $"{GetFileNameFriendly(track.BandName ?? "")} - {GetFileNameFriendly(track.Title ?? "")}{audioFormat.GetExtension()}");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            stream.CopyTo(fileStream);
        }

        public void AddItem(Stream stream, CollectionItem collectionItem, AudioFormat audioFormat)
        {
            switch (collectionItem)
            {
                case Album album:
                    AddAlbum(stream, album);
                    break;
                case Track track:
                    AddTrack(stream, track, audioFormat);
                    break;
            }
        }
    }
}
