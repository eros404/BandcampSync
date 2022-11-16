﻿using System.ComponentModel;
using Eros404.BandcampSync.Core.Models;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Settings;

public class SyncSettings : CommandSettings
{
    [CommandOption("-f|--format")]
    [DefaultValue(AudioFormat.MP3320)]
    public AudioFormat AudioFormat { get; init; }
    
    [CommandOption("-m|--manual")]
    [DefaultValue(false)]
    public bool Manual { get; init; }
}