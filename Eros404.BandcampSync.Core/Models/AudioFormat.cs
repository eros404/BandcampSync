using System.Diagnostics.CodeAnalysis;

namespace Eros404.BandcampSync.Core.Models;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AudioFormat
{
    MP3V0,
    MP3320,
    FLAC,
    AAC,
    OggVorbis,
    ALAC,
    WAV,
    AIFF
}