namespace Eros404.BandcampSync.BandcampApi.Extensions;

public static class DateTimeExtensions
{
    public static int GetTimeStamp(this DateTime dateTime) =>
        (int) dateTime.AddDays(1).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
}