namespace eShopOnTelegram.Shop.Worker.Services.Mappers;

public class CurrencyCodeToSymbolMapper
{
	private readonly Dictionary<string, char> _currencyCodesMap = new()
	{
		{ "EUR", '€' },
		{ "USD", '$' },
		{ "RUB", '₽' },
	};

	public char GetCurrencySymbol(string currencyCode)
	{
		if (_currencyCodesMap.TryGetValue(currencyCode, out var currencySymbol))
		{
			return currencySymbol;
		}

		throw new NotSupportedException($"Mapping from {currencyCode} currency code to currency symbol is not supported");
	}
}
