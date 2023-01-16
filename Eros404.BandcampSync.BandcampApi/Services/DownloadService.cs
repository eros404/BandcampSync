using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.BandcampApi.Services;

public class DownloadService : IDownloadService
{
    private readonly int _albumBatchSize;
    private readonly HttpClient _client;
    private readonly int _trackBatchSize;

    public DownloadService(IOptions<DownloadOptions> options)
    {
        _client = new HttpClient { Timeout = TimeSpan.FromMinutes(options.Value.Timeout) };
        _albumBatchSize = options.Value.AlbumBatchSize;
        _trackBatchSize = options.Value.TrackBatchSize;
    }

    public event EventHandler<DownloadStartedEventArgs>? DownloadStarted;
    public event EventHandler<DownloadFinishedEventArgs>? DownloadFinished;

    public async Task Download(IReadOnlyCollection<Album> albums)
    {
        await Download(albums, _albumBatchSize);
    }

    public async Task Download(IReadOnlyCollection<Track> tracks)
    {
        await Download(tracks, _trackBatchSize);
    }

    private async Task Download(IReadOnlyCollection<CollectionItem> items, int bashSize)
    {
        var numberOfBatches = (int)Math.Ceiling((double)items.Count / bashSize);
        for (var i = 0; i < numberOfBatches; i++)
        {
            var current = items.Skip(i * bashSize).Take(bashSize);
            var tasks = current.Select(Download);
            await Task.WhenAll(tasks);
        }
    }

    private async Task Download(CollectionItem item)
    {
        DownloadStarted?.Invoke(this, new DownloadStartedEventArgs(item));
        var response = await _client.GetAsync(item.DownloadLink);
        response.EnsureSuccessStatusCode();
        DownloadFinished?.Invoke(this, new DownloadFinishedEventArgs(item, await response.Content.ReadAsStreamAsync()));
    }
}