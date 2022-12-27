namespace Eros404.BandcampSync.Core.Services;

public interface IBandcampWebDriverFactory
{
    IBandcampWebDriver Create();
    IBandcampWebDriver CreateWithIdentity();
}