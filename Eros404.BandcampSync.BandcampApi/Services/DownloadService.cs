using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.BandcampApi.Services;

public class DownloadService : IDownloadService
{
    private readonly HttpClient _client;
    private readonly int _albumBatchSize;
    private readonly int _trackBatchSize;

    public DownloadService(IOptions<DownloadOptions> options)
    {
        _client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        _albumBatchSize = options.Value.AlbumBatchSize;
        _trackBatchSize = options.Value.TrackBatchSize;
    }

    public event EventHandler<DownloadStartedEventArgs>? DownloadStarted;
    public event EventHandler<DownloadFinishedEventArgs>? DownloadFinished;
    
    public async Task DownloadMissingAlbums(IReadOnlyCollection<Album> missingAlbums)
    {
        var numberOfAlbumBatches = (int)Math.Ceiling((double)missingAlbums.Count / _albumBatchSize);
        for (var i = 0; i < numberOfAlbumBatches; i++)
        {
            var currentAlbums = missingAlbums.Skip(i * _albumBatchSize).Take(_albumBatchSize);
            var tasks = currentAlbums.Select(album => DownloadCollectionItem(_client, album));
            await Task.WhenAll(tasks);
        }
    }
    
    public async Task DownloadMissingTracks(IReadOnlyCollection<Track> missingTracks)
    {
        var numberOfAlbumBatches = (int)Math.Ceiling((double)missingTracks.Count / _trackBatchSize);
        for (var i = 0; i < numberOfAlbumBatches; i++)
        {
            var currentTracks = missingTracks.Skip(i * _trackBatchSize).Take(_trackBatchSize);
            var tasks = currentTracks.Select(track => DownloadCollectionItem(_client, track));
            await Task.WhenAll(tasks);
        }
    }
    
    private async Task DownloadCollectionItem(HttpClient client, CollectionItem item)
    {
        DownloadStarted?.Invoke(this, new DownloadStartedEventArgs(item));
        var response = await client.GetAsync(item.DownloadLink);
        response.EnsureSuccessStatusCode();
        DownloadFinished?.Invoke(this, new DownloadFinishedEventArgs(item, await response.Content.ReadAsStreamAsync()));
    }
}