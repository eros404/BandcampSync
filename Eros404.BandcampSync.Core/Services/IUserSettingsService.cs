using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services;

public interface IUserSettingsService
{
    void UpdateValue(UserSettings key, string newValue);
    string GetValue(UserSettings key);
}