using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Orders;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

using Newtonsoft.Json;

namespace eShopOnTelegram.Shop.Worker.Commands.Orders;

/// <summary>
/// <para>Currently this command is used to create new order.
/// This command is executed when sendData(data: string) function is executed in web app</para>
/// <i>Note: sendData() function is available only when web app is opened with telegram keyboard button</i>
/// </summary>
public class CreateOrderWebAppCommand : ITelegramCommand
{
	private readonly ILogger<CreateOrderWebAppCommand> _logger;
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly OrderSummarySender _orderSummarySender;
	private readonly ChoosePaymentMethodSender _choosePaymentMethodSender;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly ITranslationsService _translationsService;
	private readonly AppSettings _appSettings;

	public CreateOrderWebAppCommand(
		ILogger<CreateOrderWebAppCommand> logger,
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		OrderSummarySender orderSummarySender,
		ChoosePaymentMethodSender choosePaymentMethodSender,
		IApplicationContentStore applicationContentStore,
		ITranslationsService translationsService,
		AppSettings appSettings)
	{
		_logger = logger;
		_telegramBot = telegramBot;
		_orderService = orderService;
		_orderSummarySender = orderSummarySender;
		_choosePaymentMethodSender = choosePaymentMethodSender;
		_applicationContentStore = applicationContentStore;
		_translationsService = translationsService;
		_appSettings = appSettings;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.Message.Chat.Id;

		try
		{
			Guard.Against.Null(update.Message);
			Guard.Against.Null(update.Message.From);
			Guard.Against.Null(update.Message.WebAppData);
			Guard.Against.Null(update.Message.WebAppData.Data);

			var createOrderRequest = JsonConvert.DeserializeObject<CreateOrderRequest>(update.Message.WebAppData.Data);

			createOrderRequest!.TelegramUserUID = update.Message.From.Id;

			var createOrderResponse = await _orderService.CreateAsync(createOrderRequest, cancellationToken: CancellationToken.None);

			if (createOrderResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("[CreateOrderWebAppCommand]: Unable to create order. {createOrderResponse}", createOrderResponse);

				await _telegramBot.SendTextMessageAsync(
					chatId,
					await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ErrorDuringOrderCreation, CancellationToken.None),
					parseMode: ParseMode.Html
				);
				return;
			}

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
		return Task.FromResult(update.Message?.Type == MessageType.WebAppData);
	}
}
