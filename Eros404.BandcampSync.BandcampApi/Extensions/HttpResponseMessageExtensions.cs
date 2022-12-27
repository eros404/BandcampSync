using System.Net.Http.Json;
using Eros404.BandcampSync.BandcampApi.Models;

namespace Eros404.BandcampSync.BandcampApi.Extensions;

public static class HttpResponseMessageExtensions
{
    internal static async Task<T?> EnsureSuccessAndReadFromJsonAsync<T>(this HttpResponseMessage response)
        where T : ErrorResponse
    {
        response.EnsureSuccessStatusCode();
        var errorContent = await response.Content.ReadFromJsonAsync<T>();
        if (errorContent is { error: true })
            throw new BandcampException(errorContent.error_message);
        return errorContent;
    }
}