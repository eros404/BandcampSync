using System;
using System.Reactive;
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

    public UserSettingsWindowViewModel(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
        email = _userSettingsService.GetValue(UserSettings.EmailAddress);
        identityCookie = "";
        SaveCommand = ReactiveCommand.Create(() => new UserSettingsModel(Email, IdentityCookie));
        SaveCommand.Subscribe(newSettings =>
        {
            _userSettingsService.UpdateValue(UserSettings.EmailAddress, newSettings.Email);
            if (!string.IsNullOrEmpty(newSettings.IdentityCookie))
            {
                _userSettingsService.UpdateValue(UserSettings.BandcampIdentityCookie, newSettings.IdentityCookie);   
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
    public ReactiveCommand<Unit, UserSettingsModel> SaveCommand { get;  }
}