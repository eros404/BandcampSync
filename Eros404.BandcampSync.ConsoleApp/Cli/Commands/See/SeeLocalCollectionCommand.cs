﻿using System.Diagnostics.CodeAnalysis;
using Eros404.BandcampSync.ConsoleApp.Cli.Settings.See;
using Eros404.BandcampSync.ConsoleApp.Extensions;
using Eros404.BandcampSync.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Eros404.BandcampSync.ConsoleApp.Cli.Commands.See
{
    internal class SeeLocalCollectionCommand : Command<SeeLocalCollectionSettings>
    {
        private readonly ILogger _logger;
        private readonly ILocalCollectionService _localCollectionService;

        public SeeLocalCollectionCommand(ILogger logger, ILocalCollectionService collectionService)
        {
            _logger = logger;
            _localCollectionService = collectionService;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] SeeLocalCollectionSettings settings)
        {
            var collection = _localCollectionService.GetLocalCollection(settings.AsAlbums);
            AnsiConsole.Write(collection.ToTable("Local Collection"));
            AnsiConsole.WriteLine(_localCollectionService.CollectionPath);
            return 0;
        }
    }
}
