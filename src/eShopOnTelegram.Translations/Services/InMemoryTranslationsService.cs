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
				{ TranslationsKeys.NoAvailableProducts, "No available products at this moment" },
				{ TranslationsKeys.AllCategories, "All categories" },
				{ TranslationsKeys.Continue, "Continue" },
				{ TranslationsKeys.ProceedToPayment, "Proceed to payment" },
			}
		},
		{
			Language.RU,
			new()
			{
				{ TranslationsKeys.NoAvailableProducts, "На данный момент нет доступных товаров" },
				{ TranslationsKeys.AllCategories, "Все категории" },
				{ TranslationsKeys.Continue, "Продолжить" },
				{ TranslationsKeys.ProceedToPayment, "Перейти к оплате" },
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
