using System.Reactive.Linq;
using System.Windows.Input;
using Eros404.BandcampSync.App.Managers;
using Eros404.BandcampSync.App.Models;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _content;
    private readonly IViewModelsManager _viewModelsManager;
    public MainWindowViewModel(IViewModelsManager viewModelsManager)
    {
        _viewModelsManager = viewModelsManager;
        SetHomeView();
    }

    private void SetHomeView()
    {
        var homeViewModel = _viewModelsManager.HomeViewModel;
        homeViewModel.SettingsUpdated += (_, _) => SetHomeView(); 
        Content = homeViewModel;
    }

    public ViewModelBase Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
}