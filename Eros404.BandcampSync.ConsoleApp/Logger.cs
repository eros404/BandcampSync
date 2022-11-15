using Eros404.BandcampSync.Core.Services;
using Spectre.Console;

namespace Eros404.BandcampSync.ConsoleApp
{
    internal class Logger : ILogger
    {
        public void LogException(Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
        }

        public void LogWarning(string message)
        {
            AnsiConsole.MarkupLine($"[orange][[Warning]] {message}[/]");
        }

        public void LogError(string message)
        {
            AnsiConsole.MarkupLine($"[red][[Error]] {message}[/]");
        }
    }
}
