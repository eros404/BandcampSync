using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.Core.Models;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class SyncViewModel : ViewModelBase
{
    private List<SelectableAlbum> _missingAlbums = new();
    private List<SelectableTrack> _missingTracks = new();

    public event EventHandler SyncCancelled;
    public event EventHandler<SyncParameters> SyncOrdered; 
    public SyncViewModel()
    {
        AudioFormat = AudioFormat.FLAC;
        AllAudioFormats = Enum.GetValues<AudioFormat>();
        CancelCommand = ReactiveCommand.Create(() => new Unit());
        CancelCommand.Subscribe(_ => SyncCancelled.Invoke(this, null));
        DownloadCommand = ReactiveCommand.Create(() => new SyncParameters(AudioFormat,
            MissingAlbums
                .Where(a => a.IsChecked)
                .Cast<Album>().ToList(),
            MissingTracks
                .Where(t => t.IsChecked)
                .Cast<Track>().ToList()));
        DownloadCommand.Subscribe(syncParameters => SyncOrdered.Invoke(this, syncParameters));
    }

    public CollectionCompareResult CompareResult
    {
        set
        {
            MissingAlbums = value.MissingAlbums
                .Select(a => new SelectableAlbum(a))
                .ToList();
            MissingTracks = value.MissingTracks
                .Select(t => new SelectableTrack(t))
                .ToList();
        }
    }
    
    public AudioFormat[] AllAudioFormats { get; }

    public AudioFormat AudioFormat { get; set; }

    public List<SelectableAlbum> MissingAlbums
    {
        get => _missingAlbums;
        set => this.RaiseAndSetIfChanged(ref _missingAlbums, value);
    }

    public List<SelectableTrack> MissingTracks
    {
        get => _missingTracks;
        set => this.RaiseAndSetIfChanged(ref _missingTracks, value);
    }

    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    public ReactiveCommand<Unit, SyncParameters> DownloadCommand { get; }
}