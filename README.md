![Github workflow badge](https://github.com/eros404/BandcampSync/actions/workflows/action.yml/badge.svg) ![NuGet badge](https://img.shields.io/nuget/v/Eros404.BandcampSync.svg)

# BandcampSync

BandcampSync is a .NET tool that make easier to download your Bandcamp collection on a local device.

Please read the [Warnings](https://github.com/eros404/BandcampSync/wiki/Warnings) before using it.

Check the [wiki](https://github.com/eros404/BandcampSync/wiki) for more detailed information.

## Installation

You can run BandcampSync on Linux, macOS, and Windows (_tested only on windows_).

You must have [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [Google Chrome](https://www.google.com/intl/en_en/chrome/) installed on your computer.

Open a shell and enter this command :

```shell
dotnet tool install --global Eros404.BandcampSync
```

## Getting Started

### Configuration

#### Identity cookie

First you need to set your identity cookie :

```shell
bandcampsync set identity "{cookie-value}"
```

You can find `cookie-value` on your navigator **when you are logged in [Bandcamp](https://www.bandcamp.com)**.

#### Local collection path

Then, you must set the path of the directory containing your local collection :

```shell
bandcampsync set local "{local-path}"
```

### Sync

To synchronize your Bandcamp collection with your local directory, use this command :

```shell
bandcampsync sync -f {download-format}
```

If you don't specify the format option, the files will be download in the format MP3 320. Available value for the format option are : `MP3V0`, `MP3320`, `FLAC`, `AAC`, `OggVorbis`, `ALAC`, `WAV`, `AIFF`.

### Expired Links

As you maybe know, some download links will be expired and Bandcamp will ask for your email to send you an other link.

Use the `set email` command to receive these mails during the synchronization :

```shell
bandcampsync set email "{your-email}"
```

Then with the fresh links received by email :

```shell
bandcampsync add -f {download-format} "{download-link-1}" "{download-link-2}" ... "{download-link-x}"
```
