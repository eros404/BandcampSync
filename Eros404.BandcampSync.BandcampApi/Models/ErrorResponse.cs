using System.Diagnostics.CodeAnalysis;

namespace Eros404.BandcampSync.BandcampApi.Models;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ErrorResponse
{
    public bool error { get; set; }
    public string error_message { get; set; } = "";
}