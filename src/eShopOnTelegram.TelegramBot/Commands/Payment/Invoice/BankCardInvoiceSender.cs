using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;

namespace eShopOnTelegram.TelegramBot.Commands.Payment.Invoice;

public class BankCardInvoiceSender : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly PaymentAppsettings _paymentAppsettings;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly ILogger<BankCardInvoiceSender> _logger;

    public BankCardInvoiceSender(
        ITelegramBotClient telegramBot,
        IProductService productService,
        IOrderService orderService,
        PaymentAppsettings paymentAppsettings,
        IApplicationContentStore applicationContentStore,
        ILogger<BankCardInvoiceSender> logger)
    {
        _telegramBot = telegramBot;
        _productService = productService;
        _orderService = orderService;
        _paymentAppsettings = paymentAppsettings;
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
                _paymentAppsettings.Card.ApiToken,
                _paymentAppsettings.MainCurrency,
                await activeOrder.CartItems.GetPaymentLabeledPricesAsync(_productService, CancellationToken.None),
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
