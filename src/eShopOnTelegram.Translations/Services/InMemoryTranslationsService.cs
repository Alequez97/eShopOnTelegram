using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;

namespace eShopOnTelegram.Translations.Services;
public class InMemoryTranslationsService : ITranslationsService
{
	private static readonly Dictionary<string, Dictionary<string, string>> _translations = new()
	{
		{
			Language.EN,
			new()
			{
				{ TranslationsKeys.Error_TryAgainLater, "Error. Try again later"},
				{ TranslationsKeys.NoAvailableProducts, "No available products at this moment" },
				{ TranslationsKeys.AllCategories, "All categories" },
				{ TranslationsKeys.Continue, "Continue" },
				{ TranslationsKeys.ProceedToPayment, "Proceed to payment" },
				{ TranslationsKeys.UnpaidOrderNotFound, "Unpaid order not found. Visit our shop to create new order" },
			}
		},
		{
			Language.RU,
			new()
			{
				{ TranslationsKeys.Error_TryAgainLater, "Ошибка. Повторите попытку позже"},
				{ TranslationsKeys.NoAvailableProducts, "На данный момент нет доступных товаров" },
				{ TranslationsKeys.AllCategories, "Все категории" },
				{ TranslationsKeys.Continue, "Продолжить" },
				{ TranslationsKeys.ProceedToPayment, "Перейти к оплате" },
				{ TranslationsKeys.UnpaidOrderNotFound, "Неоплаченный заказ не найден. Перейдите в наш магазин, чтобы создать новый заказ" },
			}
		}
	};

	public async Task<string> TranslateAsync(string language,string translationsKey, CancellationToken cancellationToken)
	{
		var translationsForLanguage = _translations[language];

		if (translationsForLanguage.TryGetValue(translationsKey, out var translation))
		{
			return await Task.FromResult(translation);
		}

		throw new NotSupportedException($"Missing {translationsKey} translations key for {language} language");
	}

	public bool IsLanguageSupported(string language)
	{
		return _translations.ContainsKey(language);
	}
}
