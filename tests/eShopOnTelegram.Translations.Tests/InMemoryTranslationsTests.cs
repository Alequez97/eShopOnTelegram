using eShopOnTelegram.Translations.Constants;
using System.Reflection;

using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Translations.Services;
using FluentAssertions;

namespace eShopOnTelegram.Translations.Tests;

public class InMemoryTranslationsTests
{
	private ITranslationsService _translationsService;

	[SetUp]
	public void Setup()
	{
		_translationsService = new InMemoryTranslationsService();
	}

	[Test]
	public void TestAllTranslationsExists()
	{
		var languageConstants = typeof(Language).GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType == typeof(string))
			.Select(f => (string)f.GetValue(null))
			.ToList();

		var translationKeys = typeof(TranslationsKeys).GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType == typeof(string))
			.Select(f => (string)f.GetValue(null))
			.ToList();

		foreach (var language in languageConstants)
		{
			foreach (var translationsKey in translationKeys)
			{
				// Call the translation service with each language and translation key constant
				var result = _translationsService.TranslateAsync(language, translationsKey, CancellationToken.None).Result;
				string.IsNullOrWhiteSpace(result).Should().BeFalse($"Translation for {language} - {translationsKey} is empty or null");
			}
		}
	}
}
