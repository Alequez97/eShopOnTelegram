﻿using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Orders;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Products;

public class ShowAvailablePaymentMethodsCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly AppSettings _appSettings;
	private readonly OrderSummarySender _orderSummarySender;
	private readonly ChoosePaymentMethodSender _choosePaymentMethodSender;
	private readonly ILogger<ShowAvailablePaymentMethodsCommand> _logger;

	public ShowAvailablePaymentMethodsCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		AppSettings appSettings,
		OrderSummarySender orderSummarySender,
		ChoosePaymentMethodSender choosePaymentMethodSender,
		ILogger<ShowAvailablePaymentMethodsCommand> logger)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_translationsService = translationsService;
		_applicationContentStore = applicationContentStore;
		_appSettings = appSettings;
		_orderSummarySender = orderSummarySender;
		_choosePaymentMethodSender = choosePaymentMethodSender;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.CallbackQuery.From.Id;

		try
		{
			var selectedProductAttributeId = update.CallbackQuery.Data.Split(InlineButtonCallbackQueryData.DataSeparator)[1];
			var quantity = update.CallbackQuery.Data.Split(InlineButtonCallbackQueryData.DataSeparator)[2];

			var createOrderRequest = new CreateOrderRequest()
			{
				TelegramUserUID = chatId,
				CartItems = new List<CreateCartItemRequest>()
				{
					new CreateCartItemRequest()
					{
						ProductAttributeId = Convert.ToInt32(selectedProductAttributeId),
						Quantity = Convert.ToInt32(quantity)
					}
				}
			};

			var createOrderResponse = await _orderService.CreateAsync(createOrderRequest, cancellationToken: CancellationToken.None);
			if (createOrderResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("Unable to create order. {createOrderResponse}", createOrderResponse);

				await _telegramBot.SendTextMessageAsync(
					chatId,
					await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ErrorDuringOrderCreation, CancellationToken.None),
					parseMode: ParseMode.Html
				);
				return;
			}

			await _telegramBot.DeleteMessageAsync(
				chatId,
				update.CallbackQuery.Message.MessageId
			);

			await _orderSummarySender.SendOrderSummaryAsync(chatId, createOrderResponse.CreatedOrder, CancellationToken.None);
			await _choosePaymentMethodSender.SendAvailablePaymentMethods(chatId, CancellationToken.None);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Contains(InlineButtonCallbackQueryData.ShowAvailablePaymentMethods));
	}
}
