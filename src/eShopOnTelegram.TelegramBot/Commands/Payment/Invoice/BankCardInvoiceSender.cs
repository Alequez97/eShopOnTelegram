using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
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
    private readonly ILogger<BankCardInvoiceSender> _logger;

    public BankCardInvoiceSender(
        ITelegramBotClient telegramBot,
        IProductService productService,
        IOrderService orderService,
        PaymentAppsettings paymentAppsettings,
        ILogger<BankCardInvoiceSender> logger)
    {
        _telegramBot = telegramBot;
        _productService = productService;
        _orderService = orderService;
        _paymentAppsettings = paymentAppsettings;
        _logger = logger;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.CallbackQuery.From.Id;

        var getOrdersResponse = await _orderService.GetByTelegramIdAsync(chatId, CancellationToken.None);

        if (getOrdersResponse.Status != ResponseStatus.Success) 
        {
            await _telegramBot.SendTextMessageAsync(chatId, "Something went wrong during invoice generation");
            return;
        }

        var customerOrders = getOrdersResponse.Data
            .Where(order => order.Status == OrderStatus.New.ToString() || order.Status == OrderStatus.InvoiceSent.ToString())
            .ToList();

        if (customerOrders.Count > 1)
        {
            var errorMessage = "Error. For every customer should be only one order with status new";
            
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }

        if (customerOrders.Count == 0)
        {
            _logger.LogError("Error. No active order found for customer with telegramId = {telegramId}", chatId);
            throw new Exception($"Error. No active order found for customer with telegramId = {chatId}");
        }

        var activeOrder = customerOrders.First();

        await _telegramBot.SendInvoiceAsync(
            chatId,
            $"Заказ номер {activeOrder.OrderNumber}",
            "", // Description - maybe worth to add list of purchasing products
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

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.BankCard);
    }
}
