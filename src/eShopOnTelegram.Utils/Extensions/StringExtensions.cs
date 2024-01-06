namespace eShopOnTelegram.Utils.Extensions;
public static class StringExtensions
{
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        return char.ToLowerInvariant(str[0]) + str[1..];
    }

    public static string? ToNullIfEmptyOrWhiteSpace(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return null;
        }

        return str;
    }
}
