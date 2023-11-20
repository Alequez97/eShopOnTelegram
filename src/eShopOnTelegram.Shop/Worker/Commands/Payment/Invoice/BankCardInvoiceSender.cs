using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Worker.Commands.Interfaces;
using eShopOnTelegram.Shop.Worker.Constants;
using eShopOnTelegram.Shop.Worker.Extensions;
using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.Shop.Worker.Commands.Payment.Invoice;

public class BankCardInvoiceSender : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IProductAttributeService _productAttributeService;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly PaymentSettings _paymentSettings;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly ILogger<BankCardInvoiceSender> _logger;

    public BankCardInvoiceSender(
        ITelegramBotClient telegramBot,
        IProductAttributeService productAttributeService,
        IOrderService orderService,
        IPaymentService paymentService,
        AppSettings appSettings,
        IApplicationContentStore applicationContentStore,
        ILogger<BankCardInvoiceSender> logger)
    {
        _telegramBot = telegramBot;
        _productAttributeService = productAttributeService;
        _orderService = orderService;
        _paymentService = paymentService;
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

            await _paymentService.UpdateOrderPaymentMethod(activeOrder.OrderNumber, OrderPaymentMethod.Card);
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
