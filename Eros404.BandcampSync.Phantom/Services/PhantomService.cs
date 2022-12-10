using System.Text.Json;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;

namespace Eros404.BandcampSync.Phantom.Services;

public class PhantomService : IPhantomService
{
    private readonly string _phantomFilePath;
    private readonly Collection _phantoms;

    public PhantomService(string collectionPath)
    {
        _phantomFilePath = Path.Combine(collectionPath, ".phantoms");
        _phantoms = !File.Exists(_phantomFilePath)
            ? new Collection()
            : JsonSerializer.Deserialize<Collection>(File.ReadAllText(_phantomFilePath)) ?? new Collection();
    }

    public Collection GetPhantoms() => _phantoms;

    public void AddPhantom(CollectionItem item)
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

    public void AddPhantoms(List<CollectionItem> items) => items.ForEach(AddPhantom);

    public void RemovePhantom(CollectionItem item)
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
        if (!File.Exists(_phantomFilePath))
        {
            File.Create(_phantomFilePath).Dispose();
        }
        File.WriteAllText(_phantomFilePath, JsonSerializer.Serialize(_phantoms));
    }
}