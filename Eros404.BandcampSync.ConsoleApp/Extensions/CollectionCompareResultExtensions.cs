using Eros404.BandcampSync.Core.Models;
using Spectre.Console;

namespace Eros404.BandcampSync.ConsoleApp.Extensions;

public static class CollectionCompareResultExtensions
{
    public static Table ToTable(this CollectionCompareResult compareResult, string title)
    {
        compareResult.SortItems();

        var table = new Table
        {
            Title = new TableTitle(title, new Style(Color.Red))
        };
        table.AddColumn(new TableColumn("main").Centered());
        table.HideHeaders();

        if (compareResult.MissingAlbums.Any())
            table.AddRow(compareResult.MissingAlbums.ToTable());

        if (compareResult.MissingAlbums.Any() && compareResult.MissingTracks.Any())
            table.AddRow("");

        if (compareResult.MissingTracks.Any())
            table.AddRow(compareResult.MissingTracks.ToTable());

        if (!compareResult.MissingAlbums.Any() && !compareResult.MissingTracks.Any())
            table.AddRow("No missing items.");

        return table;
    }
}