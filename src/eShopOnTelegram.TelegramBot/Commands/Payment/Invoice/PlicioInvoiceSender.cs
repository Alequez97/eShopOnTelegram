using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio;
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
            var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);

            if (getOrdersResponse.Status != ResponseStatus.Success)
            {
                await _telegramBot.SendTextMessageAsync(chatId, _botContentAppsettings.Order.InvoiceGenerationFailedErrorMessage.OrNextIfNullOrEmpty(BotContentDefaultConstants.Order.InvoiceGenerationFailedErrorMessage));
                return;
            }

            var activeOrder = getOrdersResponse.Data;

            var createPlicioInvoiceResponse = await _plicioClient.CreateInvoiceAsync(
                _paymentAppsettings.Plisio.ApiToken,
                _paymentAppsettings.MainCurrency,
                (int)Math.Ceiling(activeOrder.TotalPrice),
                activeOrder.OrderNumber,
                _paymentAppsettings.Plisio.CryptoCurrency);

            var message = _botContentAppsettings.Payment.ProceedToPayment.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.ProceedToPayment);

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new []
                {
                    InlineKeyboardButton.WithUrl(_botContentAppsettings.Payment.ProceedToPayment.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.ProceedToPayment), createPlicioInvoiceResponse.Data.InvoiceUrl),
                },
            });

            await _telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: _botContentAppsettings.Payment.InvoiceReceiveMessage.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.InvoiceReceiveMessage),
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
