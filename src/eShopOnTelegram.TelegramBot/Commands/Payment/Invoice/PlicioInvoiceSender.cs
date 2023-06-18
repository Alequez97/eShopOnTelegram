using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Commands.Payment.Invoice;

public class PlicioInvoiceSender : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IPlicioClient _plicioClient;
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly PaymentAppsettings _paymentAppsettings;
    private readonly BotContentAppsettings _botContentAppsettings;
    private readonly ILogger<PlicioInvoiceSender> _logger;

    public PlicioInvoiceSender(
        ITelegramBotClient telegramBot,
        IPlicioClient plicioClient,
        IOrderService orderService,
        IProductService productService,
        PaymentAppsettings paymentAppsettings,
        BotContentAppsettings botContentAppsettings,
        ILogger<PlicioInvoiceSender> logger)
    {
        _telegramBot = telegramBot;
        _plicioClient = plicioClient;
        _orderService = orderService;
        _productService = productService;
        _paymentAppsettings = paymentAppsettings;
        _botContentAppsettings = botContentAppsettings;
        _logger = logger;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.CallbackQuery.From.Id;

        try
        {
            var getOrdersResponse = await _orderService.GetByTelegramIdAsync(chatId, CancellationToken.None);

            if (getOrdersResponse.Status != ResponseStatus.Success)
            {
                await _telegramBot.SendTextMessageAsync(chatId, _botContentAppsettings.Order.InvoiceGenerationFailedErrorMessage ?? BotContentDefaultConstants.Order.InvoiceGenerationFailedErrorMessage);
                return;
            }

            var customerOrders = getOrdersResponse.Data
                .Where(order => order.Status == OrderStatus.New.ToString() || order.Status == OrderStatus.InvoiceSent.ToString())
                .ToList();

            if (customerOrders.Count > 1)
            {
                var errorMessage = "Error. For every customer should be only one active order";

                _logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            if (customerOrders.Count == 0)
            {
                _logger.LogError("Error. No active order found for customer with telegramId = {telegramId}", chatId);
                throw new Exception($"Error. No active order found for customer with telegramId = {chatId}");
            }

            var activeOrder = customerOrders.First();

            var createPlicioInvoiceResponse = await _plicioClient.CreateInvoiceAsync(
                _paymentAppsettings.Plisio.ApiToken,
                _paymentAppsettings.MainCurrency,
                (int)Math.Ceiling(activeOrder.TotalPrice),
                activeOrder.OrderNumber,
                _paymentAppsettings.Plisio.CryptoCurrency);

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new []
                {
                    InlineKeyboardButton.WithUrl("Proceed to payment", createPlicioInvoiceResponse.Data.InvoiceUrl),
                },
            });

            await _telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: "Please receive your invoice",
                replyMarkup: inlineKeyboard,
                cancellationToken: CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendCommonErrorMessageAsync(chatId, _botContentAppsettings, CancellationToken.None);
        }
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.Plicio);
    }
}
