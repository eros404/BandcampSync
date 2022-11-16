﻿using System.ComponentModel;
using Eros404.BandcampSync.Core.Models;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings;

public class SyncSettings : CommandSettings
{
    [CommandOption("-f|--format")]
    [DefaultValue(AudioFormat.MP3320)]
    [Description("Default value is MP3V0. Other values: MP3320, FLAC, AAC, OggVorbis, ALAC, WAV, AIFF")]
    public AudioFormat AudioFormat { get; init; }
}