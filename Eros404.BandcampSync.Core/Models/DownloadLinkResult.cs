namespace Eros404.BandcampSync.Core.Models;

public class DownloadLinkResult
{
    public readonly string? DownloadLink;
    public readonly bool HasExpired;
    public readonly bool InvalidEmail;

    private DownloadLinkResult(string? downloadLink, bool hasExpired, bool invalidEmail)
    {
        DownloadLink = downloadLink;
        HasExpired = hasExpired;
        InvalidEmail = invalidEmail;
    }

    public static DownloadLinkResult Expired => new(null, true, false);
    public static DownloadLinkResult WrongEmail => new(null, true, true);

    public static DownloadLinkResult Success(string downloadLink)
    {
        return new(downloadLink, false, false);
    }
}