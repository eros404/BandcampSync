﻿namespace Eros404.BandcampSync.Core.Models;

public class MissingAlbum : Album, IComparable<MissingAlbum>
{
    public MissingAlbum()
    {
    }

    public MissingAlbum(Album album, uint numberOfMissingTracks)
    {
        Title = album.Title;
        BandName = album.BandName;
        NumberOfTracks = album.NumberOfTracks;
        NumberOfMissingTracks = numberOfMissingTracks;
        RedownloadUrl = album.RedownloadUrl;
    }

    public uint NumberOfMissingTracks { get; set; }

    public int CompareTo(MissingAlbum? other)
    {
        return base.CompareTo(other);
    }
}