using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.Mail.Services;

public class MailService : IMailService
{
    public MailService(IUserSettingsService userSettingsService)
    {
        EmailAddress = userSettingsService.GetValue(UserSettings.EmailAddress);
    }

    public string EmailAddress { get; }
}