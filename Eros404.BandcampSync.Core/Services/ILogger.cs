namespace Eros404.BandcampSync.Core.Services
{
    public interface ILogger
    {
        void LogWarning(string message);
        void LogException(Exception ex);
        void LogError(string message);
    }
}
