using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Eros404.BandcampSync.App.Models;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(UserSettingsWindowViewModel userSettingsWindowViewModel)
    {
        ShowUserSettingsDialog = new Interaction<UserSettingsWindowViewModel, UserSettingsModel>();
        ShowUserSettingsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await ShowUserSettingsDialog.Handle(userSettingsWindowViewModel);
        });
    }
    public ICommand ShowUserSettingsCommand { get;  }
    public Interaction<UserSettingsWindowViewModel, UserSettingsModel> ShowUserSettingsDialog { get; }
}