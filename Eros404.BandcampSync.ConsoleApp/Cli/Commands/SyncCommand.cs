﻿using Eros404.BandcampSync.ConsoleApp.Cli.Settings;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands;

internal class SyncCommand : AsyncCommand<SyncSettings>
{
    private readonly ILogger _logger;
    private readonly IBandcampApiService _bandCampService;
    private readonly ILocalCollectionService _localCollectionService;
    private readonly IBandcampWebDriverFactory _webDriverFactory;
    private readonly IMailService _mailService;

    public SyncCommand(ILogger logger, IBandcampApiService bandCampService, ILocalCollectionService localCollectionService, IBandcampWebDriverFactory webDriverFactory, IMailService mailService)
    {
        _logger = logger;
        _bandCampService = bandCampService;
        _localCollectionService = localCollectionService;
        _webDriverFactory = webDriverFactory;
        _mailService = mailService;
    }

    private int _numberOfItemDownloaded;
    private int _numberOfNewLinkSent;

    public override async Task<int> ExecuteAsync(CommandContext context, SyncSettings settings)
    {
        var compareResult = await CompareCollectionsCommand.CompareAsync(_bandCampService, _localCollectionService);
        if (compareResult == null)
            return -1;
        AnsiConsole.Write(compareResult.ToTable("Missing Items"));
        if (!compareResult.MissingAlbums.Any() && !compareResult.MissingTracks.Any())
            return 0;
        
        var selectedAlbums = SelectAlbumsToDownload(compareResult.MissingAlbums);
        var selectedTracks = SelectTracksToDownload(compareResult.MissingTracks);
        if (!selectedAlbums.Any() && !selectedTracks.Any())
            return 0;
        
        using var webDriver = _webDriverFactory.CreateWithIdentity();
        using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        await AnsiConsole.Status()
            .StartAsync("Downloading...", async _ =>
            {
                foreach (var album in selectedAlbums)
                    await DownloadAlbum(webDriver, client, album, settings.AudioFormat);
                
                foreach (var track in selectedTracks)
                    await DownloadTrack(webDriver, client, track, settings.AudioFormat);
            });
        AnsiConsole.MarkupLine(
            $"[green]{_numberOfItemDownloaded}[/] item{(_numberOfItemDownloaded > 1 ? "s" : "")} downloaded.");
        if (_numberOfNewLinkSent > 0)
        {
            AnsiConsole.MarkupLine(
                $"[green]{_numberOfNewLinkSent}[/] link{(_numberOfNewLinkSent > 1 ? "s" : "")} have been sent to {_mailService.EmailAddress}.");
        }
        return 0;
    }

    private async Task DownloadAlbum(IBandcampWebDriver webDriver, HttpClient client, Album album,
        AudioFormat audioFormat)
    {
        await DownloadCollectionItem(webDriver, client, album, album.ToString(), audioFormat,
            stream => _localCollectionService.AddAlbum(stream, album));
    }
    private async Task DownloadTrack(IBandcampWebDriver webDriver, HttpClient client, Track track,
        AudioFormat audioFormat)
    {
        await DownloadCollectionItem(webDriver, client, track, track.ToString(), audioFormat,
            stream => _localCollectionService.AddTrack(stream, track, audioFormat));
    }
    private async Task DownloadCollectionItem(IBandcampWebDriver webDriver, HttpClient client, CollectionItem item,
        string itemDisplayName, AudioFormat audioFormat, Action<Stream> addToCollectionAction)
    {
        AnsiConsole.MarkupLine($"Preparing [blue]{itemDisplayName.EscapeMarkup()}[/] for download.");
        var result = webDriver.GetDownloadLink(item.RedownloadUrl ?? "", audioFormat, _mailService.EmailAddress);
        if (result.HasExpired)
        {
            AnsiConsole.MarkupLine($"Download link for [blue]{itemDisplayName.EscapeMarkup()}[/] expired.");
            if (result.InvalidEmail)
                AnsiConsole.MarkupLine($"The email address {_mailService.EmailAddress} was rejected by Bandcamp.");
            else
            {
                AnsiConsole.MarkupLine($"New link sent.");
                _numberOfNewLinkSent++;
            }
        }
        else
        {
            AnsiConsole.MarkupLine($"Downloading [blue]{itemDisplayName.EscapeMarkup()}[/].");
            var response = await client.GetAsync(result.DownloadLink);
            response.EnsureSuccessStatusCode();
            addToCollectionAction(await response.Content.ReadAsStreamAsync());
            _numberOfItemDownloaded++;
        }
    }

    private static List<MissingAlbum> SelectAlbumsToDownload(List<MissingAlbum> albums)
    {
        if (!albums.Any())
            return new List<MissingAlbum>();
        var dictionary = albums.ToDictionary(a => a.ToString(), a => a);
        var all = $"All albums ({albums.Count})";
        var selectedKeys = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[blue]Select albums to download:[/]")
                .NotRequired()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more albums)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle an album, [green]<enter>[/] to accept)[/]")
                .AddChoices(all)
                .AddChoices(dictionary.Keys));
        return selectedKeys.Contains(all)
            ? albums
            : selectedKeys.Select(key => dictionary[key]).ToList();
    }
    private static List<MissingTrack> SelectTracksToDownload(List<MissingTrack> tracks)
    {
        if (!tracks.Any())
            return new List<MissingTrack>();
        var dictionary = tracks.ToDictionary(track => track.ToString(), a => a);
        var all = $"All tracks ({tracks.Count})";
        var selectedKeys = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[blue]Select tracks to download:[/]")
                .NotRequired()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more tracks)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a track, [green]<enter>[/] to accept)[/]")
                .AddChoices(all)
                .AddChoices(dictionary.Keys));
        return selectedKeys.Contains(all)
            ? tracks
            : selectedKeys.Select(key => dictionary[key]).ToList();
    }
}