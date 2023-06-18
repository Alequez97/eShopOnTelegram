using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Services.Payment.Interfaces;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Services.Telegram;

public class PaymentMethodsSender
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IProductService _productService;
    private readonly IEnumerable<IPaymentTelegramButtonProvider> _paymentTelegramButtonGenerators;
    private readonly BotContentAppsettings _botContentAppsettings;
    private readonly PaymentAppsettings _paymentAppsettings;

    public PaymentMethodsSender(
        ITelegramBotClient telegramBot,
        IProductService productService,
        IEnumerable<IPaymentTelegramButtonProvider> paymentTelegramButtonGenerators,
        BotContentAppsettings botContentAppsettings,
        PaymentAppsettings paymentAppsettings)
    {
        _telegramBot = telegramBot;
        _productService = productService;
        _paymentTelegramButtonGenerators = paymentTelegramButtonGenerators;
        _botContentAppsettings = botContentAppsettings;
        _paymentAppsettings = paymentAppsettings;
    }

    public async Task SendEnabledPaymentMethodsAsync(long chatId, CancellationToken cancellationToken)
    {
        if (_paymentAppsettings.AllPaymentsDisabled)
        {
            await _telegramBot.SendTextMessageAsync(chatId, _botContentAppsettings.Payment.NoEnabledPayments.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.NoEnabledPayments));
            return;
        }

        var paymentMethodButtons = _paymentTelegramButtonGenerators
            .Where(buttonGenerator => buttonGenerator.PaymentMethodEnabled(_paymentAppsettings))
            .Select(buttonGenerator => new List<InlineKeyboardButton>() { buttonGenerator.GetInvoiceGenerationButton() });

        InlineKeyboardMarkup inlineKeyboard = new(paymentMethodButtons);

        await _telegramBot.SendTextMessageAsync(
            chatId: chatId,
            text: _botContentAppsettings.Payment.ChoosePaymentMethod.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.ChoosePaymentMethod),
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}
