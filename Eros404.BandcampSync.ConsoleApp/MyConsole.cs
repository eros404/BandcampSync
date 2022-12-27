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

    public static List<T> SelectItems<T>(List<T> items, string title, bool includeAllOption)
    {
        if (!items.Any())
            return new List<T>();
        var dictionary = items.ToDictionary(i => (i?.ToString() ?? "").EscapeMarkup(), i => i);
        var allOption = $"All items ({items.Count})";
        var prompt = new MultiSelectionPrompt<string>()
            .Title($"[blue]{title.EscapeMarkup()}[/]")
            .NotRequired()
            .PageSize(12)
            .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to toggle an item, [green]<enter>[/] to accept)[/]");
        if (includeAllOption)
            prompt.AddChoices(allOption);
        prompt.AddChoices(dictionary.Keys);
        var selectedKeys = AnsiConsole.Prompt(prompt);
        return selectedKeys.Contains(allOption)
            ? items.ToList()
            : selectedKeys.Select(key => dictionary[key]).ToList();
    }
}