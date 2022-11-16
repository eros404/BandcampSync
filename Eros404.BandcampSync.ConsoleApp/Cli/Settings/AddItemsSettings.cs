using System.ComponentModel;
using Eros404.BandcampSync.Core.Models;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings;

public class AddItemsSettings : CommandSettings
{
    [CommandArgument(0, "<downloadLink>")]
    [Description("Download links provided by Bandcamp. Usually sent by mail.")]
    public string[]? RedownLoadUrls { get; set; }
    
    [CommandOption("-f|--format")]
    [DefaultValue(AudioFormat.MP3320)]
    [Description("Default value is MP3V0. Other values: MP3320, FLAC, AAC, OggVorbis, ALAC, WAV, AIFF")]
    public AudioFormat AudioFormat { get; init; }
}