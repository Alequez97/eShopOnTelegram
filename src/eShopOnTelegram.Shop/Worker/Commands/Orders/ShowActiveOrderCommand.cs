using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Responses.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Exceptions;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Orders;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments;
using eShopOnTelegram.Shop.Worker.Services.Telegram.MessageSenders.Payments.Invoices.Interfaces;
using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Orders;

public class ShowActiveOrderCommand : ITelegramCommand
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IOrderService _orderService;
	private readonly ITranslationsService _translationsService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly IEnumerable<IInvoiceSender> _invoiceSenders;
	private readonly OrderSummarySender _orderSummarySender;
	private readonly ChoosePaymentMethodSender _choosePaymentMethodSender;
	private readonly AppSettings _appSettings;
	private readonly ILogger<ShowActiveOrderCommand> _logger;

	public ShowActiveOrderCommand(
		ITelegramBotClient telegramBot,
		IOrderService orderService,
		ITranslationsService translationsService,
		IApplicationContentStore applicationContentStore,
		IEnumerable<IInvoiceSender> invoiceSenders,
		OrderSummarySender orderSummarySender,
		ChoosePaymentMethodSender choosePaymentMethodSender,
		AppSettings appSettings,
		ILogger<ShowActiveOrderCommand> logger)
	{
		_telegramBot = telegramBot;
		_orderService = orderService;
		_translationsService = translationsService;
		_applicationContentStore = applicationContentStore;
		_invoiceSenders = invoiceSenders;
		_orderSummarySender = orderSummarySender;
		_choosePaymentMethodSender = choosePaymentMethodSender;
		_appSettings = appSettings;
		_logger = logger;
	}

	public async Task SendResponseAsync(Update update)
	{
		var chatId = update.Message.Chat.Id;

		try
		{
			var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

			if (getOrdersResponse.Status == ResponseStatus.NotFound)
			{
				await _telegramBot.SendTextMessageAsync(chatId, await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.UnpaidOrderNotFound, CancellationToken.None));
				return;
			}

			if (getOrdersResponse.Status != ResponseStatus.Success)
			{
				_logger.LogError("[ShowActiveOrderCommand]: Can't get order by telegram id");
				await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
				return;
			}
			var activeOrder = getOrdersResponse.Data;

			await _orderSummarySender.SendOrderSummaryAsync(chatId, activeOrder, CancellationToken.None);

			if (activeOrder.PaymentMethodSelected)
			{
				var invoiceSender = _invoiceSenders.FirstOrDefault(invoiceSender => invoiceSender.PaymentMethod.ToString() == activeOrder.PaymentMethod);
				
				if (invoiceSender == null)
				{
					throw new InvoiceSenderNotFoundException(activeOrder.PaymentMethod);
				}

				await invoiceSender.SendInvoiceAsync(chatId, activeOrder, CancellationToken.None);
			}
			else
			{
				await _choosePaymentMethodSender.SendAvailablePaymentMethods(chatId, CancellationToken.None);
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
		}
	}

	public async Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return update.Message?.Text?.Contains(await _translationsService.TranslateAsync(_appSettings.Language, TranslationsKeys.ShowUnpaidOrder, CancellationToken.None)) ?? false;
	}
}
