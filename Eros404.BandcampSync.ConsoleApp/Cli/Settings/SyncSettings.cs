using System.ComponentModel;
using Eros404.BandcampSync.Core.Models;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings;

public class SyncSettings : CommandSettings
{
    [CommandOption("-f|--format")]
    [DefaultValue(AudioFormat.MP3320)]
    [Description("Accepted values are `MP3320` (default), `MP3V0`, `FLAC`, `AAC`, `OggVorbis`, `ALAC`, `WAV`, `AIFF`")]
    public AudioFormat AudioFormat { get; init; }
}