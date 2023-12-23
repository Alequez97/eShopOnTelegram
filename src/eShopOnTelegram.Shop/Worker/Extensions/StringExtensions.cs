namespace eShopOnTelegram.Shop.Worker.Extensions;

public static class StringExtensions
{
    public static string OrNextIfNullOrEmpty(this string value, string alternative)
    {
        return string.IsNullOrWhiteSpace(value) ? alternative : value;
    }
}
