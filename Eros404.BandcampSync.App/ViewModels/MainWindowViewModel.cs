using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(UserSettingsViewModel userSettings)
    {
        UserSettings = userSettings;
    }

    public UserSettingsViewModel UserSettings { get; set; }
}