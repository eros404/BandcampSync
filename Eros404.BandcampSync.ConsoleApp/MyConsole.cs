using Spectre.Console;

namespace Eros404.BandcampSync.ConsoleApp;

public static class MyConsole
{
    public static async Task<string> SelectionAsync(Dictionary<string, Func<Task>> actions, string title)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .AddChoices(actions.Keys)
        );
        await actions[selection]();
        return selection;
    }
    public static string Selection(Dictionary<string, Action> actions, string title)
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .AddChoices(actions.Keys)
        );
        actions[selection]();
        return selection;
    }
}