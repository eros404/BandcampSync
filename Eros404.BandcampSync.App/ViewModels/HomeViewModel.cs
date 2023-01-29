using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Eros404.BandcampSync.App.Managers;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using ReactiveUI;
using UserSettings = Eros404.BandcampSync.App.Models.UserSettings;

namespace Eros404.BandcampSync.App.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private const string CompareButtonTextInitial = "Find missing items";
    private const string CompareButtonTextLoading = "Loading...";
    
    private string _compareButtonText;

    public event EventHandler SettingsUpdated;
    public event EventHandler<CollectionCompareResult> CompareResultReceived; 
    public HomeViewModel(IViewModelsManager viewModelsManager, IComparatorService comparatorService)
    {
        ShowUserSettingsDialog = new Interaction<UserSettingsWindowViewModel, UserSettings>();
        ShowUserSettingsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await ShowUserSettingsDialog.Handle(viewModelsManager.UserSettingsWindowViewModel);
            SettingsUpdated.Invoke(this, null);
        });
        _compareButtonText = CompareButtonTextInitial;
        CompareCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CompareButtonText = CompareButtonTextLoading;
            try
            {
                var compareResult = await comparatorService.CompareLocalWithBandcamp();
                CompareResultReceived.Invoke(this, compareResult);
            }
            catch (Exception e)
            {

            }
            finally
            {
                _compareButtonText = CompareButtonTextInitial;
            }
        });
    }
    public ICommand ShowUserSettingsCommand { get;  }
    public Interaction<UserSettingsWindowViewModel, UserSettings> ShowUserSettingsDialog { get; }

    public string CompareButtonText
    {
        get => _compareButtonText;
        set => this.RaiseAndSetIfChanged(ref _compareButtonText, value);
    }
    public ICommand CompareCommand { get; }
}