using eShopOnTelegram.TelegramBot.Services.Mappers.Enums;
using System.Text.RegularExpressions;
using System.Text;

namespace eShopOnTelegram.TelegramBot.Services.Mappers;

public class CurrencyCodeToSymbolMapper
{
    private readonly Dictionary<CurrencyCode, string> _currencyCodesMap = new()
    {
        { CurrencyCode.EUR, "€" },
        { CurrencyCode.USD, "$" },
        { CurrencyCode.RUB, "₽" },
    };

    public string GetEmojiUnicode(CurrencyCode key)
    {
        if (_currencyCodesMap.TryGetValue(key, out var currencySymbol))
        {
            return Encoding.UTF8.GetString(Array.ConvertAll(Regex.Unescape(currencySymbol).ToCharArray(), c => (byte)c));
        }

        return string.Empty;
    }
}
