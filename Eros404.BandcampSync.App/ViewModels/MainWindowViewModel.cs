using System.Reactive.Linq;
using System.Windows.Input;
using Eros404.BandcampSync.App.Managers;
using Eros404.BandcampSync.App.Models;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(IViewModelsManager viewModelsManager)
    {
        ShowUserSettingsDialog = new Interaction<UserSettingsWindowViewModel, UserSettingsModel>();
        ShowUserSettingsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await ShowUserSettingsDialog.Handle(viewModelsManager.UserSettingsWindowViewModel);
        });
    }
    public ICommand ShowUserSettingsCommand { get;  }
    public Interaction<UserSettingsWindowViewModel, UserSettingsModel> ShowUserSettingsDialog { get; }
}