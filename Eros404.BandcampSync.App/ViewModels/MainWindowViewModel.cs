using Eros404.BandcampSync.App.Managers;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.Core.Models;
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
        homeViewModel.CompareResultReceived += (_, compareResult) => SetSyncView(compareResult);
        Content = homeViewModel;
    }

    private void SetSyncView(CollectionCompareResult compareResult)
    {
        var syncViewModel = _viewModelsManager.SyncViewModel;
        syncViewModel.CompareResult = compareResult;
        syncViewModel.SyncCancelled += (_, _) => SetHomeView();
        syncViewModel.SyncOrdered += (_, syncParameters) => SetDownloadView(syncParameters);
        Content = syncViewModel;
    }

    private void SetDownloadView(SyncParameters syncParameters)
    {
        
    }

    public ViewModelBase Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
}