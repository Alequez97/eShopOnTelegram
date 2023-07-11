using System.Text;

using eshopOnTelegram.TelegramBot.Appsettings;

using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.Persistence.Entities;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Extensions;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Commands.Payment;

public class SuccessfulPaymentCommand : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly IApplicationContentStore _applicationContentStore;
    private readonly IBotOwnerDataStore _botOwnerDataStore;
    private readonly TelegramAppsettings _telegramAppsettings;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SuccessfulPaymentCommand> _logger;

    public SuccessfulPaymentCommand(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        IApplicationContentStore applicationContentStore,
        IBotOwnerDataStore botOwnerDataStore,
        TelegramAppsettings telegramAppsettings,
        IConfiguration configuration,
        ILogger<SuccessfulPaymentCommand> logger)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _applicationContentStore = applicationContentStore;
        _botOwnerDataStore = botOwnerDataStore;
        _telegramAppsettings = telegramAppsettings;
        _configuration = configuration;
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

                InlineKeyboardMarkup inlineKeyboard = null;
                var orderReceivedMessage = new StringBuilder("New order received!!!");

                if (!string.IsNullOrEmpty(_configuration["AdminHost"]))
                {
                    var orderLink = $"{_configuration["AdminHost"]}/#/orders/{update.Message.SuccessfulPayment.InvoicePayload}";

                    orderReceivedMessage
                        .AppendLine()
                        .AppendLine("Click button for details");

                    inlineKeyboard = new(new[]
                    {
                        InlineKeyboardButton.WithUrl(
                            text: "Show order details",
                            url: orderLink)
                    });
                }

                var telegramGroupId = await _botOwnerDataStore.GetBotOwnerTelegramGroupIdAsync(CancellationToken.None);
                    await _telegramBot.SendTextMessageAsync(
                        chatId: string.IsNullOrWhiteSpace(telegramGroupId) ? _telegramAppsettings.BotOwnerTelegramId : telegramGroupId,
                        text: orderReceivedMessage.ToString(),
                        parseMode: ParseMode.Html,
                        replyMarkup: inlineKeyboard
                    );
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