using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Extensions;

public static class AudioFormatExtensions
{
    public static string GetExtension(this AudioFormat audioFormat) => audioFormat switch
        {
            AudioFormat.MP3V0 => ".mp3",
            AudioFormat.MP3320 => ".mp3",
            AudioFormat.FLAC => ".flac",
            AudioFormat.AAC => ".aac",
            AudioFormat.OggVorbis => ".ogg",
            AudioFormat.ALAC => ".m4a",
            AudioFormat.WAV => ".wav",
            AudioFormat.AIFF => ".aiff",
            _ => string.Empty
        };
    public static string GetDisplayName(this AudioFormat audioFormat) => audioFormat switch
    {
        AudioFormat.MP3V0 => "MP3 V0",
        AudioFormat.MP3320 => "MP3 320",
        AudioFormat.FLAC => "FLAC",
        AudioFormat.AAC => "AAC",
        AudioFormat.OggVorbis => "Ogg Vorbis",
        AudioFormat.ALAC => "ALAC",
        AudioFormat.WAV => "WAV",
        AudioFormat.AIFF => "AIFF",
        _ => string.Empty
    };
}