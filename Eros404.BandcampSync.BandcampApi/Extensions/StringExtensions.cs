using System.Text;

namespace Eros404.BandcampSync.BandcampApi.Extensions;

public static class StringExtensions
{
    public static string KeepOnlyNumericCharacters(this string input)
    {
        var sb = new StringBuilder();
        foreach (var c in input.Where(c => c is >= '0' and <= '9'))
            sb.Append(c);
        return sb.ToString();
    }
}