﻿using System.Text;

using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Services.Mappers;
using eShopOnTelegram.TelegramBot.Services.Payment.Interfaces;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Services.Telegram;

public class PaymentProceedMessageSender
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IOrderService _orderService;
    private readonly IEnumerable<IPaymentTelegramButtonProvider> _paymentTelegramButtonGenerators;
    private readonly CurrencyCodeToSymbolMapper _currencyCodeToSymbolMapper;
    private readonly BotContentAppsettings _botContentAppsettings;
    private readonly PaymentAppsettings _paymentAppsettings;

    public PaymentProceedMessageSender(
        ITelegramBotClient telegramBot,
        IOrderService orderService,
        IEnumerable<IPaymentTelegramButtonProvider> paymentTelegramButtonGenerators,
        CurrencyCodeToSymbolMapper currencyCodeToSymbolMapper,
        BotContentAppsettings botContentAppsettings,
        PaymentAppsettings paymentAppsettings)
    {
        _telegramBot = telegramBot;
        _orderService = orderService;
        _paymentTelegramButtonGenerators = paymentTelegramButtonGenerators;
        _currencyCodeToSymbolMapper = currencyCodeToSymbolMapper;
        _botContentAppsettings = botContentAppsettings;
        _paymentAppsettings = paymentAppsettings;
    }

    public async Task SendProceedToPaymentAsync(long chatId, OrderDto order, CancellationToken cancellationToken)
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

        var message = new StringBuilder();
        var currencySymbol = _currencyCodeToSymbolMapper.GetCurrencySymbol(_paymentAppsettings.MainCurrency);

        message
            .AppendLine($"<b>{_botContentAppsettings.Order.OrderSummaryTitle.OrNextIfNullOrEmpty(BotContentDefaultConstants.Order.OrderSummaryTitle)}</b>")
            .AppendLine(new string('~', 20));

        foreach (var orderCartItem in order.CartItems)
        {
            message
            .AppendLine($"{orderCartItem.Name} (x{orderCartItem.Quantity}) {orderCartItem.TotalPrice}{currencySymbol}");
        };

        message
            .AppendLine()
            .AppendLine($"{_botContentAppsettings.Order.TotalPriceTitle.OrNextIfNullOrEmpty(BotContentDefaultConstants.Order.TotalPriceTitle)}: <b>{order.TotalPrice}{currencySymbol}</b>")
            .AppendLine(new string('~', 20));

        message
            .AppendLine(_botContentAppsettings.Payment.ChoosePaymentMethod.OrNextIfNullOrEmpty(BotContentDefaultConstants.Payment.ChoosePaymentMethod));

        await _telegramBot.SendTextMessageAsync(
            chatId: chatId,
            text: message.ToString(),
            parseMode: ParseMode.Html,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}
