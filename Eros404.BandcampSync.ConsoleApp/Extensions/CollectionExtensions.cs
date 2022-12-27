using Eros404.BandcampSync.Core.Models;
using Spectre.Console;

namespace Eros404.BandcampSync.ConsoleApp.Extensions;

public static class CollectionExtensions
{
    public static Table ToTable(this Collection collection, string title)
    {
        collection.SortItems();

        var table = new Table
        {
            Title = new TableTitle(title, new Style(Color.Red))
        };
        table.AddColumn(new TableColumn("main").Centered());
        table.HideHeaders();

        if (collection.Albums.Any())
            table.AddRow(collection.Albums.ToTable());

        if (collection.Albums.Any() && collection.Tracks.Any())
            table.AddRow("");

        if (collection.Tracks.Any())
            table.AddRow(collection.Tracks.ToTable());

        if (!collection.Albums.Any() && !collection.Tracks.Any())
            table.AddRow("This collection is empty");

        return table;
    }
}