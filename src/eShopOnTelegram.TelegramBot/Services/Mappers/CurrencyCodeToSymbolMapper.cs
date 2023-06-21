using eShopOnTelegram.TelegramBot.Services.Mappers.Enums;
using System.Text.RegularExpressions;
using System.Text;

namespace eShopOnTelegram.TelegramBot.Services.Mappers;

public class CurrencyCodeToSymbolMapper
{
    private readonly Dictionary<string, char> _currencyCodesMap = new()
    {
        { "EUR", '€' },
        { "USD", '$' },
        { "RUB", '₽' },
    };

    public char GetCurrencySymbol(string key)
    {
        if (_currencyCodesMap.TryGetValue(key, out var currencySymbol))
        {
            return currencySymbol;
        }

        return ' ';
    }
}
