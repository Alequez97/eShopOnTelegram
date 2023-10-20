using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Worker.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Worker.Constants;
using eShopOnTelegram.TelegramBot.Worker.Extensions;

namespace eShopOnTelegram.TelegramBot.Worker.Commands.Payment.Invoice;

public class BankCardInvoiceSender : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IProductAttributeService _productAttributeService;
    private readonly IOrderService _orderService;
    private readonly PaymentSettings _paymentSettings;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly ILogger<BankCardInvoiceSender> _logger;

    public BankCardInvoiceSender(
        ITelegramBotClient telegramBot,
        IProductAttributeService productAttributeService,
        IOrderService orderService,
        AppSettings appSettings,
        IApplicationContentStore applicationContentStore,
        ILogger<BankCardInvoiceSender> logger)
    {
        _telegramBot = telegramBot;
        _productAttributeService = productAttributeService;
        _orderService = orderService;
        _paymentSettings = appSettings.PaymentSettings;
        _applicationContentStore = applicationContentStore;
        _logger = logger;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.CallbackQuery.From.Id;

        try
        {
            var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

            if (getOrdersResponse.Status != ResponseStatus.Success)
            {
                await _telegramBot.SendTextMessageAsync(chatId, await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.InvoiceGenerationFailedErrorMessage, CancellationToken.None));
                return;
            }

            var activeOrder = getOrdersResponse.Data;

            await _telegramBot.SendInvoiceAsync(
                chatId,
                await _applicationContentStore.GetValueAsync(ApplicationContentKey.Order.OrderNumberTitle, CancellationToken.None),
                "Description", // TODO: Add list of purchasing products
                activeOrder.OrderNumber,
                _paymentSettings.Card.ApiToken,
                _paymentSettings.MainCurrency,
                await activeOrder.CartItems.GetPaymentLabeledPricesAsync(_productAttributeService, CancellationToken.None),
                needShippingAddress: true,
                needPhoneNumber: true,
                needName: true,
                cancellationToken: CancellationToken.None
            );

            await _orderService.UpdateStatusAsync(activeOrder.OrderNumber, OrderStatus.InvoiceSent, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.BankCard));
    }
}
