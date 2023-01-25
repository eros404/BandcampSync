using System;
using System.Reactive;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class UserSettingsViewModel : ViewModelBase
{
    private readonly IUserSettingsService _userSettingsService;
    private string email;
    private string identityCookie;

    public UserSettingsViewModel(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
        email = _userSettingsService.GetValue(UserSettings.EmailAddress);
        identityCookie = _userSettingsService.GetValue(UserSettings.BandcampIdentityCookie);
        Save = ReactiveCommand.Create(() => new UserSettingsModel(Email, IdentityCookie));
        Save.Subscribe(newSettings =>
        {
            _userSettingsService.UpdateValue(UserSettings.EmailAddress, newSettings.Email);
            _userSettingsService.UpdateValue(UserSettings.BandcampIdentityCookie, newSettings.IdentityCookie);
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
    public ReactiveCommand<Unit, UserSettingsModel> Save { get;  }
}