namespace Eros404.BandcampSync.App.Models;

public class UserSettingsModel
{
    public UserSettingsModel(string email, string identityCookie)
    {
        Email = email;
        IdentityCookie = identityCookie;
    }

    public string Email { get; set; }
    public string IdentityCookie { get; set; }
}