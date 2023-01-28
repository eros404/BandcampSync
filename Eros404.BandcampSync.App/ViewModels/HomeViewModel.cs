using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Eros404.BandcampSync.App.Managers;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.Core.Services;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private string _compareButtonText;

    public event EventHandler SettingsUpdated;
    public HomeViewModel(IViewModelsManager viewModelsManager, IComparatorService comparatorService)
    {
        ShowUserSettingsDialog = new Interaction<UserSettingsWindowViewModel, UserSettingsModel>();
        ShowUserSettingsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await ShowUserSettingsDialog.Handle(viewModelsManager.UserSettingsWindowViewModel);
            SettingsUpdated.Invoke(this, null);
        });
        _compareButtonText = "Find missing items";
        CompareCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CompareButtonText = "Loading";
            try
            {
                var compareResult = await comparatorService.CompareLocalWithBandcamp();
                CompareButtonText = "Done";
            }
            catch (Exception e)
            {
                CompareButtonText = e.Message;
            }
        });
    }
    public ICommand ShowUserSettingsCommand { get;  }
    public Interaction<UserSettingsWindowViewModel, UserSettingsModel> ShowUserSettingsDialog { get; }

    public string CompareButtonText
    {
        get => _compareButtonText;
        set => this.RaiseAndSetIfChanged(ref _compareButtonText, value);
    }
    public ICommand CompareCommand { get; }
}