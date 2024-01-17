using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment;

public class PreCheckoutQueryCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly AppSettings _appSettings;
	private readonly ILogger<PreCheckoutQueryCommand> _logger;

	public PreCheckoutQueryCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		AppSettings appSettings,
		ILogger<PreCheckoutQueryCommand> logger)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_translationsService = translationsService;
		_applicationContentStore = applicationContentStore;
		_appSettings = appSettings;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var preCheckoutQuery = update.PreCheckoutQuery;
		var chatId = preCheckoutQuery.From.Id;

		try
		{
			var getOrderResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);
			if (getOrderResponse.Status == ResponseStatus.Success)
			{
				var activeOrder = getOrderResponse.Data;

				if (activeOrder.OrderNumber != preCheckoutQuery.InvoicePayload)
				{
					// This is case when user generated multiple invoices in chat and by mistake choose old invoice to pay for

					var messageText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.IncorrectInvoiceChoosen, CancellationToken.None);
					await _telegramBot.SendTextMessageAsync(chatId, messageText);
					return;
				}

				if (activeOrder.HasBeenPaid)
				{
					var messageText = await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OrderAlreadyHasBeenPaid, CancellationToken.None);
					await _telegramBot.SendTextMessageAsync(chatId, messageText);
					return;
				}

				await _telegramBot.AnswerPreCheckoutQueryAsync(preCheckoutQuery.Id);
				return;
			}

			await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.OrderAlreadyHasBeenPaid, CancellationToken.None));
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.PreCheckoutQuery != null);
	}
}
