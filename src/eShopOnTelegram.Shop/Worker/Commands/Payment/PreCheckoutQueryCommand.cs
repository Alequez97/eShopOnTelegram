using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment;

public class PreCheckoutQueryCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public PreCheckoutQueryCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		IApplicationContentStore applicationContentStore,
		ITranslationsService translationsService,
		AppSettings appSettings
		)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_applicationContentStore = applicationContentStore;
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task SendResponseAsync(Update update)
	{
		var preCheckoutQuery = update.PreCheckoutQuery;

		var getOrderResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(preCheckoutQuery.From.Id, CancellationToken.None);
		if (getOrderResponse.Status == ResponseStatus.Success)
		{
			if (getOrderResponse.Data.TotalPrice * 100 != update.PreCheckoutQuery.TotalAmount)
			{
				// This is case when user generated multiple invoices in chat 
				// and by mistake choose incorrect invoice and will pay less or more, than in active order

				var messageText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.IncorrectInvoiceChoosen, CancellationToken.None);
				await _telegramBot.SendTextMessageAsync(preCheckoutQuery.From.Id, messageText);
				return;
			}

			await _telegramBot.AnswerPreCheckoutQueryAsync(preCheckoutQuery.Id);
			return;
		}

		await _telegramBot.SendTextMessageAsync(preCheckoutQuery.From.Id, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.AlreadyPaidOrExpired, CancellationToken.None));
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.PreCheckoutQuery != null);
	}
}
