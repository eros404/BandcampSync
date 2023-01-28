using Eros404.BandcampSync.App.ViewModels;

namespace Eros404.BandcampSync.App.Managers;

public interface IViewModelsManager
{
    UserSettingsWindowViewModel UserSettingsWindowViewModel { get; }
    MainWindowViewModel MainWindowViewModel { get; }
}