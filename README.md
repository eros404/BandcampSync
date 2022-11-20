# BandcampSync

## Introduction

BandcampSync is a tool that make easier to download your Bandcamp collection on a local device.

### Warnings

This application need a authentication cookie to work. With this cookie, anybody can access your Bandcamp account freely.

You just have to trust me when I say that I don't collect any data with this cookie or use it with malicious purpose. You can also read the code of course.

In addition this cookie is stored on your computer in a not encrypted json file, so I encourage you to delete it when you are done :

```shell
BandcampSync set identity ""
```

## Getting Started

### Installation

[Click here](https://github.com/eros404/BandcampSync/releases/download/v0.1.0-beta.1/BandcampSync.zip) to download the **Beta 1** release.

BandcampSync is cross-platform and framework dependent.

So you can run it on Linux, macOS, and Windows but you must have [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed on your computer (_tested only on windows_).

Having [Google Chrome](https://www.google.com/intl/en_en/chrome/) installed on your computer is also required.

### Configuration

#### Identity cookie

First you need to set your identity cookie :

```shell
BandcampSync set identity "{cookie-value}"
```

You can find `cookie-value` on your navigator **when you are logged in [Bandcamp](https://www.bandcamp.com)**.

With Firefox :

1. Press F12
2. Open the tab "Storage"
3. Under "Cookies", click on "https://bandcamp.com"
4. Find the cookie with the name "identity" and copy its value

Now you should see your Bandcamp collection with this command :

```shell
BandcampSync see bandcamp
```

#### Local collection path

Then, you must set the path of the directory containing your local collection :

```shell
BandcampSync set local "{local-path}"
```

Now you should see your local collection with this command :

```shell
BandcampSync see local
```

### Sync

To synchronize your Bandcamp collection with your local directory, use this command :

```shell
BandcampSync sync -f {download-format}
```

If you don't specify the format option, the files will be download in the format MP3 320. Available value for the format option are : `MP3V0`, `MP3320`, `FLAC`, `AAC`, `OggVorbis`, `ALAC`, `WAV`, `AIFF`.

The program will show you the missing items in your local collection (like with the `compare` command). Then you will have to choose the items you want to download.

### Expired Links

As you maybe know, some download links will be expired and Bandcamp will ask for your email to send you an other link.

In order to receive these mails during the synchronization, use this command :

```shell
BandcampSync set email "{your-email}"
```

With the fresh links received by email, use this command :

```shell
BandcampSync add -f {download-format} "{download-link-1}" "{download-link-2}" ... "{download-link-x}"
```

## Resources

### Documentation

[bandcamp-api-docs](https://github.com/har-nick/bandcamp-api-docs) by har-nick

[Bandcamp-API](https://michaelherger.github.io/Bandcamp-API/) by Michael Herger

### External libraries

[Spectre.Console](https://github.com/spectreconsole/spectre.console), a .NET library to create beautiful, cross platform, console applications.

[TagLib#](https://github.com/mono/taglib-sharp), a .NET library for reading and writing metadata in media files.

[Selenium WebDriver](https://www.selenium.dev/documentation/webdriver/) that drives a browser natively.
