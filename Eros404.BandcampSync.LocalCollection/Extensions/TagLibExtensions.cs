using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.LocalCollection.Extensions
{
    internal static class TagLibExtensions
    {
        public static Track ToTrack(this TagLib.File file)
        {
            return new Track
            {
                Title = file.Tag.Title,
                Number = file.Tag.Track,
                AlbumTitle = file.Tag.Album,
                BandName = string.Join(" ", file.Tag.Performers)
            };
        }
        public static List<Track> ToTracks(this IEnumerable<TagLib.File> files) =>
            files.Select(file => file.ToTrack()).ToList();
        public static List<Album> ToAlbums(this IEnumerable<TagLib.File> files)
        {
            var albumDictionnary = new Dictionary<string, Album>();
            foreach (var file in files)
            {
                if (albumDictionnary.TryGetValue(file.Tag.Album, out var album))
                    album.NumberOfTracks++;
                else
                    albumDictionnary[file.Tag.Album] = new Album
                    {
                        Title = file.Tag.Album,
                        BandName = string.Join(" ", file.Tag.AlbumArtists),
                        NumberOfTracks = 1
                    };
            }
            return albumDictionnary.Values.ToList();
        }
    }
}
