namespace eShopOnTelegram.Translations.Interfaces;

public interface ITranslationsService
{
	public Task<string> TranslateAsync(string language, string translationsKey, CancellationToken cancellationToken);

	public bool IsLanguageSupported(string language);
}
