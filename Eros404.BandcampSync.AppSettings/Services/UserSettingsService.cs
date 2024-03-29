﻿using Eros404.BandcampSync.AppSettings.Extensions;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eros404.BandcampSync.AppSettings.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly string _filePath;
    private readonly IDataProtector _protector;
    private readonly Dictionary<string, string> _userSettings;

    public UserSettingsService(IDataProtectionProvider dataProtectionProvider, string filePath)
    {
        if (!File.Exists(filePath)) File.Create(filePath).Dispose();
        _userSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath)) ??
                        new Dictionary<string, string>();
        _protector = dataProtectionProvider.CreateProtector("BandcampSync.UserSettings");
        _filePath = filePath;
    }

    public string GetValue(UserSettings key)
    {
        return key.IsEncrypted()
            ? GetEncryptedValueOrEmptyString(key.ToString())
            : GetValueOrEmptyString(key.ToString());
    }

    public void UpdateValue(UserSettings key, string newValue)
    {
        if (key.IsEncrypted())
            UpdateEncryptedValue(key.ToString(), newValue);
        else
            UpdateValue(key.ToString(), newValue);
    }

    private string GetValueOrEmptyString(string key)
    {
        return _userSettings.TryGetValue(key, out var value) ? value : "";
    }

    private string GetEncryptedValueOrEmptyString(string key)
    {
        return _userSettings.TryGetValue(key, out var value) ? _protector.Unprotect(value) : "";
    }

    private void UpdateValue(string key, string newValue)
    {
        _userSettings[key] = newValue;
        var settingsObject = JObject.FromObject(_userSettings);
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(settingsObject, Formatting.Indented));
    }

    private void UpdateEncryptedValue(string key, string newValue)
    {
        UpdateValue(key, _protector.Protect(newValue));
    }
}