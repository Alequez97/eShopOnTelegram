using eShopOnTelegram.Shop.Api.Constants;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Api.Endpoints.Config;

public class GetClientSideConfig : EndpointBaseAsync
	.WithoutRequest
	.WithActionResult<GetTranslationsResponse>
{
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public GetClientSideConfig(
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	[HttpGet("/api/translations")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Config })]
	public override async Task<ActionResult<GetTranslationsResponse>> HandleAsync(CancellationToken cancellationToken)
	{
		var language = _appSettings.Language;
		var isLanguageSupported = _translationsService.IsLanguageSupported(language);

		if (!isLanguageSupported)
		{
			throw new NotSupportedException($"{language} language is not supported");
		}

		var clientSideConfig = new GetTranslationsResponse
		{
			NoAvailableProducts = await _translationsService.TranslateAsync(language, TranslationsKeys.NoAvailableProducts, cancellationToken),
			AllCategories = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.AllCategories, cancellationToken),
			Continue = await _translationsService.TranslateAsync(language, TranslationsKeys.Continue, cancellationToken),
			ProceedToPayment = await _translationsService.TranslateAsync(language, TranslationsKeys.ProceedToPayment, cancellationToken),
		};

		return Ok(clientSideConfig);
	}
}

public class GetTranslationsResponse
{
	public required string NoAvailableProducts { get; set; }
	public required string AllCategories { get; set; }
	public required string Continue { get; set; }
	public required string ProceedToPayment { get; set; }
}
