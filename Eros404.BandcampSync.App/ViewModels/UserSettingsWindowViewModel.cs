using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class UserSettingsWindowViewModel : ViewModelBase
{
    private readonly IUserSettingsService _userSettingsService;
    private string email;
    private string identityCookie;
    private string localCollectionPath;

    public UserSettingsWindowViewModel(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
        email = _userSettingsService.GetValue(UserSettings.EmailAddress);
        identityCookie = "";
        localCollectionPath = _userSettingsService.GetValue(UserSettings.LocalCollectionPath);
        SaveCommand = ReactiveCommand.Create(() => new UserSettingsModel(LocalCollectionPath, Email, IdentityCookie));
        SaveCommand.Subscribe(newSettings =>
        {
            _userSettingsService.UpdateValue(UserSettings.LocalCollectionPath, newSettings.LocalCollectionPath);
            _userSettingsService.UpdateValue(UserSettings.EmailAddress, newSettings.Email);
            if (!string.IsNullOrEmpty(newSettings.IdentityCookie))
            {
                _userSettingsService.UpdateValue(UserSettings.BandcampIdentityCookie, newSettings.IdentityCookie);   
            }
        });
        SelectLocalCollectionPathDialog = new Interaction<Unit, string>();
        SelectLocalCollectionPathCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var result = await SelectLocalCollectionPathDialog.Handle(new Unit());
            if (result is not null)
            {
                LocalCollectionPath = result;
            }
        });
    }
    public string Email
    {
        get => email;
        set => this.RaiseAndSetIfChanged(ref email, value);
    }
    public string IdentityCookie
    {
        get => identityCookie;
        set => this.RaiseAndSetIfChanged(ref identityCookie, value);
    }
    public string LocalCollectionPath
    {
        get => localCollectionPath;
        set => this.RaiseAndSetIfChanged(ref localCollectionPath, value);
    }
    public ReactiveCommand<Unit, UserSettingsModel> SaveCommand { get; }
    public ICommand SelectLocalCollectionPathCommand { get; }
    public Interaction<Unit, string?> SelectLocalCollectionPathDialog { get; }
}