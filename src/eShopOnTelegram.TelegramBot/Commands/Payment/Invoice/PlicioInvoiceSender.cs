using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;

using Refit;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Commands.Payment.Invoice;

public class PlicioInvoiceSender : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IPlicioClient _plicioClient;
    private readonly IOrderService _orderService;
    private readonly PaymentAppsettings _paymentAppsettings;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly ILogger<PlicioInvoiceSender> _logger;

    public PlicioInvoiceSender(
        ITelegramBotClient telegramBot,
        IPlicioClient plicioClient,
        IOrderService orderService,
        PaymentAppsettings paymentAppsettings,
        IApplicationContentStore applicationContentStore,
        ILogger<PlicioInvoiceSender> logger)
    {
        _telegramBot = telegramBot;
        _plicioClient = plicioClient;
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
                    InlineKeyboardButton.WithUrl(await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.ProceedToPayment, CancellationToken.None), createPlicioInvoiceResponse.Data.InvoiceUrl),
                },
            });

            await _telegramBot.SendTextMessageAsync(
                chatId: chatId,
                text: await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.InvoiceReceiveMessage, CancellationToken.None),
                replyMarkup: inlineKeyboard,
                cancellationToken: CancellationToken.None);
        }
        catch (ApiException apiException)
        {
            _logger.LogError(apiException, $"{apiException.Message}\n{apiException.Content}");
            await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.Plicio));
    }
}
