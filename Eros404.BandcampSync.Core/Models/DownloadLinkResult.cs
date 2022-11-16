namespace Eros404.BandcampSync.Core.Models;

public class DownloadLinkResult
{
    private DownloadLinkResult(string? downloadLink, bool hasExpired, bool invalidEmail)
    {
        DownloadLink = downloadLink;
        HasExpired = hasExpired;
        InvalidEmail = invalidEmail;
    }

    public readonly string? DownloadLink;
    public readonly bool HasExpired;
    public readonly bool InvalidEmail;

    public static DownloadLinkResult Success(string downloadLink) => new(downloadLink, false, false);
    public static DownloadLinkResult Expired => new(null, true, false);
    public static DownloadLinkResult WrongEmail => new(null, true, true);
}