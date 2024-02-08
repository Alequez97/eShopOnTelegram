﻿using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Notifications.Interfaces;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly IPaymentService _paymentService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ITranslationsService _translationsService;
	private readonly IEnumerable<INotificationSender> _notificationSenders;
	private readonly ILogger<SuccessfulPaymentCommand> _logger;
	private readonly AppSettings _appSettings;

	public SuccessfulPaymentCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		IPaymentService paymentService,
		IApplicationContentStore applicationContentStore,
		ITranslationsService translationsService,
		IEnumerable<INotificationSender> notificationSenders,
		ILogger<SuccessfulPaymentCommand> logger,
		AppSettings appSettings)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_paymentService = paymentService;
		_applicationContentStore = applicationContentStore;
		_translationsService = translationsService;
		_notificationSenders = notificationSenders;
		_logger = logger;
		_appSettings = appSettings;
	}

	public async Task SendResponseAsync(Update update)
	{
		Guard.Against.Null(update.Message);
		Guard.Against.Null(update.Message.SuccessfulPayment);

		var chatId = update.Message.Chat.Id;

		try
		{
			var orderNumber = update.Message.SuccessfulPayment.InvoicePayload;

			var response = await _paymentService.ConfirmOrderPaymentAsync(orderNumber, PaymentMethod.Card, CancellationToken.None);

			if (response.Status != ResponseStatus.Success)
			{
				throw new Exception("Failed to confirm order payment in SuccessfulPayment TG Command.");
			}

			await _telegramBot.SendTextMessageAsync(
				chatId,
				await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.SuccessfullPayment, CancellationToken.None)
			);

			foreach (var notificationSender in _notificationSenders)
			{
				await notificationSender.SendOrderReceivedNotificationAsync(orderNumber, CancellationToken.None);
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Message?.Type == MessageType.SuccessfulPayment);
	}
}
