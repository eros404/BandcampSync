namespace Eros404.BandcampSync.BandcampApi.Models;

public class BandcampException : Exception
{
    public BandcampException(string? message) : base(message)
    {}
}