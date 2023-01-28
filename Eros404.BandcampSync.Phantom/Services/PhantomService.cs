using System.Text.Json;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;

namespace Eros404.BandcampSync.Phantom.Services;

public class PhantomService : IPhantomService
{
    private const string PhantomFileName = ".phantoms";

    private readonly string _phantomFilePath = "";
    private readonly IUserSettingsService? _userSettingsService;
    private Collection _phantoms;

    public PhantomService(string collectionPath)
    {
        _phantomFilePath = Path.Combine(collectionPath, PhantomFileName);
        InitializePhantoms();
    }
    public PhantomService(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
        InitializePhantoms();
    }

    private void InitializePhantoms()
    {
        var phantomFilePath = PhantomFilePath;
        _phantoms = !File.Exists(phantomFilePath)
            ? new Collection()
            : JsonSerializer.Deserialize<Collection>(File.ReadAllText(phantomFilePath)) ?? new Collection();
    }

    private string PhantomFilePath => _userSettingsService is null
        ? _phantomFilePath
        : Path.Combine(_userSettingsService.GetValue(UserSettings.LocalCollectionPath), PhantomFileName);

    public Collection GetPhantoms()
    {
        return _phantoms;
    }

    public void AddPhantoms(params CollectionItem[] items)
    {
        Array.ForEach(items, AddPhantom);
    }

    public void RemovePhantoms(params CollectionItem[] items)
    {
        Array.ForEach(items, RemovePhantom);
    }

    private void AddPhantom(CollectionItem item)
    {
        switch (item)
        {
            case Album album:
                if (!_phantoms.Albums.Contains(album))
                    _phantoms.Albums.Add(album);
                break;
            case Track track:
                if (!_phantoms.Tracks.Contains(track))
                    _phantoms.Tracks.Add(track);
                break;
        }

        SavePhantoms();
    }

    private void RemovePhantom(CollectionItem item)
    {
        switch (item)
        {
            case Album album:
                _phantoms.Albums.Remove(album);
                break;
            case Track track:
                _phantoms.Tracks.Remove(track);
                break;
        }

        SavePhantoms();
    }

    private void SavePhantoms()
    {
        var phantomFilePath = PhantomFilePath;
        if (!File.Exists(phantomFilePath)) File.Create(phantomFilePath).Dispose();
        File.WriteAllText(phantomFilePath, JsonSerializer.Serialize(_phantoms));
    }
}