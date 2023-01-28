using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Eros404.BandcampSync.App.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using ReactiveUI;

namespace Eros404.BandcampSync.App.ViewModels;

public class UserSettingsWindowViewModel : ViewModelBase
{
    private string _email;
    private string _identityCookie;
    private string _localCollectionPath;

    public UserSettingsWindowViewModel(IUserSettingsService userSettingsService)
    {
        _email = userSettingsService.GetValue(UserSettings.EmailAddress);
        _identityCookie = "";
        _localCollectionPath = userSettingsService.GetValue(UserSettings.LocalCollectionPath);
        SaveCommand = ReactiveCommand.Create(() => new UserSettingsModel(LocalCollectionPath, Email, IdentityCookie));
        SaveCommand.Subscribe(newSettings =>
        {
            userSettingsService.UpdateValue(UserSettings.LocalCollectionPath, newSettings.LocalCollectionPath);
            userSettingsService.UpdateValue(UserSettings.EmailAddress, newSettings.Email);
            if (!string.IsNullOrEmpty(newSettings.IdentityCookie))
            {
                userSettingsService.UpdateValue(UserSettings.BandcampIdentityCookie, newSettings.IdentityCookie);   
            }
        });
        SelectLocalCollectionPathDialog = new Interaction<Unit, string?>();
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
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }
    public string IdentityCookie
    {
        get => _identityCookie;
        set => this.RaiseAndSetIfChanged(ref _identityCookie, value);
    }
    public string LocalCollectionPath
    {
        get => _localCollectionPath;
        set => this.RaiseAndSetIfChanged(ref _localCollectionPath, value);
    }
    public ReactiveCommand<Unit, UserSettingsModel> SaveCommand { get; }
    public ICommand SelectLocalCollectionPathCommand { get; }
    public Interaction<Unit, string?> SelectLocalCollectionPathDialog { get; }
}