using System;
using Eros404.BandcampSync.App.ViewModels;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Eros404.BandcampSync.App.Managers;

public class ViewModelsManager : IViewModelsManager
{
    private readonly IServiceProvider _serviceProvider;

    public ViewModelsManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private T GetService<T>()
    {
        var result = _serviceProvider.GetService<T>();
        if (result is null)
            throw new Exception($"Could not resolve service {typeof(T)}");
        return result;
    }

    public MainWindowViewModel MainWindowViewModel => new (this);
    public UserSettingsWindowViewModel UserSettingsWindowViewModel => new (GetService<IUserSettingsService>());
    public HomeViewModel HomeViewModel => new(this, GetService<IComparatorService>());
    public SyncViewModel SyncViewModel => new();
}