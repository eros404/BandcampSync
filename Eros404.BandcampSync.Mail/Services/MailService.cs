using Eros404.BandcampSync.Core.Models;
using Eros404.BandcampSync.Core.Services;

namespace Eros404.BandcampSync.Mail.Services;

public class MailService : IMailService
{
    public MailService(IUserSettingsService userSettingsService)
    {
        EmailAddress = userSettingsService.GetValue(UserSettings.EmailAddress);
    }

    public string EmailAddress { get; }
}