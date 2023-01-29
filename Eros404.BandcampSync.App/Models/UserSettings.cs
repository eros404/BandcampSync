namespace Eros404.BandcampSync.App.Models;

public class UserSettings
{
    public UserSettings(string localCollectionPath, string email, string identityCookie)
    {
        Email = email;
        IdentityCookie = identityCookie;
        LocalCollectionPath = localCollectionPath;
    }

    public string Email { get; set; }
    public string IdentityCookie { get; set; }
    public string LocalCollectionPath { get; set; }
}