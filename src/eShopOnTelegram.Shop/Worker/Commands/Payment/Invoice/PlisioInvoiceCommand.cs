﻿using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.Persistence.Entities.Payments;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Refit;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;

public class PlisioInvoiceCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IPlisioClient _plisioClient;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly AppSettings _appSettings;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ILogger<PlisioInvoiceCommand> _logger;

	public PlisioInvoiceCommand(
		ITelegramBotClient telegramBot,
		IPlisioClient plisioClient,
		IOrderService orderService,
		IPaymentService paymentService,
		AppSettings appSettings,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		ILogger<PlisioInvoiceCommand> logger)
	{
		_telegramBot = telegramBot;
		_plisioClient = plisioClient;
		_orderService = orderService;
		_paymentService = paymentService;
		_translationsService = translationsService;
		_appSettings = appSettings;
		_applicationContentStore = applicationContentStore;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var telegramId = update.CallbackQuery.From.Id;

		try
		{
			var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(telegramId, CancellationToken.None);

			if (getOrdersResponse.Status != ResponseStatus.Success)
			{
				await _telegramBot.SendTextMessageAsync(telegramId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
				return;
			}
			if (getOrdersResponse.Data.PaymentMethodSelected)
			{
				await _telegramBot.SendTextMessageAsync(telegramId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.PaymentMethodAlreadySelected, CancellationToken.None));
				return;
			}

			var activeOrder = getOrdersResponse.Data;

			var createPlisioInvoiceResponse = await _plisioClient.CreateInvoiceAsync(
				_appSettings.PaymentSettings.Plisio.ApiToken,
				_appSettings.PaymentSettings.MainCurrency,
				(int)Math.Ceiling(activeOrder.TotalPrice),
				activeOrder.OrderNumber,
				_appSettings.PaymentSettings.Plisio.CryptoCurrency,
				$"{_appSettings.TelegramBotSettings.ShopAppUrl}/api/webhook/plisio?json=true",
				telegramId.ToString());

			var updatePaymentResponse = await _paymentService.UpdateOrderPaymentMethodAsync(activeOrder.OrderNumber, PaymentMethod.Plisio, CancellationToken.None);
			var updateInvoiceUrlResponse = await _paymentService.UpdateInvoiceUrlAsync(activeOrder.OrderNumber, createPlisioInvoiceResponse.Data.InvoiceUrl, CancellationToken.None);
			if (updatePaymentResponse.Status != ResponseStatus.Success || updateInvoiceUrlResponse.Status != ResponseStatus.Success)
			{
				throw new Exception($"[{nameof(PlisioInvoiceCommand)}]: Failed to update order payment data.");
			}

			activeOrder.InvoiceUrl = createPlisioInvoiceResponse.Data.InvoiceUrl;

			var plisioInvoiceSender = new PlisioInvoiceSender(_telegramBot, _translationsService, _appSettings);
			await plisioInvoiceSender.SendInvoiceAsync(telegramId, activeOrder, CancellationToken.None);
		}
		catch (ApiException apiException)
		{
			_logger.LogError(apiException, $"{apiException.Message}\n{apiException.Content}");
			await _telegramBot.SendDefaultErrorMessageAsync(telegramId, _applicationContentStore, _logger, CancellationToken.None);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(telegramId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.Plisio));
	}
}
