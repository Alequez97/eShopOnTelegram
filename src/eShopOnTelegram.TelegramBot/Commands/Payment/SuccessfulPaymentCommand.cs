using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly BotContentAppsettings _botContentAppsettings;
    private readonly ILogger<SuccessfulPaymentCommand> _logger;

    public SuccessfulPaymentCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        BotContentAppsettings botContentAppsettings,
        ILogger<SuccessfulPaymentCommand> logger)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _botContentAppsettings = botContentAppsettings;
        _logger = logger;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.Message.Chat.Id;

        try
        {
            var orderNumber = update.Message.SuccessfulPayment.InvoicePayload;

            var response = await _orderService.UpdateStatusAsync(orderNumber, OrderStatus.Paid, CancellationToken.None);

            if (response.Status == ResponseStatus.Success)
            {
                await _telegramBot.SendTextMessageAsync(
                    chatId,
                    _botContentAppsettings.Payment.SuccessfullPayment.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.SuccessfullPayment)
                );

                // TODO: Send notification to shop owner, that new order received
            }
            else
            {
                await _telegramBot.SendTextMessageAsync(
                    chatId,
                    _botContentAppsettings.Payment.ErrorDuringPaymentConfirmation.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.ErrorDuringPaymentConfirmation),
                    ParseMode.Html
                );
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendCommonErrorMessageAsync(chatId, _botContentAppsettings, CancellationToken.None);
        }
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Message?.Type == MessageType.SuccessfulPayment;
    }
}