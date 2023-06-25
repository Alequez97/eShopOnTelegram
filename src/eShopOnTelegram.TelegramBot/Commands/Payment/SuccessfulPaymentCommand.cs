using eshopOnTelegram.TelegramBot.Appsettings;

using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Extensions;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly IBotOwnerDataStore _botOwnerDataStore;
    private readonly TelegramAppsettings _telegramAppsettings;
    private readonly ILogger<SuccessfulPaymentCommand> _logger;

    public SuccessfulPaymentCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        IApplicationContentStore applicationContentStore,
        IBotOwnerDataStore botOwnerDataStore,
        TelegramAppsettings telegramAppsettings,
        ILogger<SuccessfulPaymentCommand> logger)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _applicationContentStore = applicationContentStore;
        _botOwnerDataStore = botOwnerDataStore;
        _telegramAppsettings = telegramAppsettings;
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
                    await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.SuccessfullPayment, CancellationToken.None)
                );

                var telegramGroupId = await _botOwnerDataStore.GetBotOwnerTelegramGroupIdAsync(CancellationToken.None);

                if (!string.IsNullOrWhiteSpace(telegramGroupId))
                {
                    await _telegramBot.SendTextMessageAsync(
                        chatId: telegramGroupId,
                        text: "New order received!!!",
                        parseMode: ParseMode.Html
                    );
                }
                else
                {
                    await _telegramBot.SendTextMessageAsync(
                        chatId: _telegramAppsettings.BotOwnerTelegramId,
                        text: "New order received!!!",
                        parseMode: ParseMode.Html
                    );
                }
            }
            else
            {
                await _telegramBot.SendTextMessageAsync(
                    chatId,
                    await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.ErrorDuringPaymentConfirmation, CancellationToken.None),
                    parseMode: ParseMode.Html
                );
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await _telegramBot.SendDefaultErrorMessageAsync(chatId, _applicationContentStore, _logger, CancellationToken.None);
        }
    }

    public Task<bool> IsResponsibleForUpdateAsync(Update update)
    {
        return Task.FromResult(update.Message?.Type == MessageType.SuccessfulPayment);
    }
}