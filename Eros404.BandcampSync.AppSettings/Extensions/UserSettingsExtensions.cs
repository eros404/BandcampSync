using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.AppSettings.Extensions;

public static class UserSettingsExtensions
{
    internal static bool IsEncrypted(this UserSettings setting)
    {
        return setting switch
        {
            UserSettings.BandcampIdentityCookie => true,
            _ => false
        };
    }
}