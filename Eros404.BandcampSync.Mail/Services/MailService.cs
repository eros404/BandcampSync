using Eros404.BandcampSync.AppSettings.Models;
using Eros404.BandcampSync.Core.Services;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.Mail.Services;

public class MailService : IMailService
{
    private readonly string _emailAddress;

    public MailService(IOptions<EmailOptions> options)
    {
        _emailAddress = options.Value.Address;
    }

    public string EmailAddress => _emailAddress;
}