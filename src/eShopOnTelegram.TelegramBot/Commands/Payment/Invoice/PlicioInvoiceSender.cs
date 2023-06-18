using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;

namespace eShopOnTelegram.TelegramBot.Commands.Payment.Invoice;

public class PlicioInvoiceSender : ITelegramCommand
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IProductService _productService;
    private readonly PaymentAppsettings _paymentAppsettings;

    public PlicioInvoiceSender(
        ITelegramBotClient telegramBot,
        IProductService productService,
        PaymentAppsettings paymentAppsettings)
    {
        _telegramBot = telegramBot;
        _productService = productService;
        _paymentAppsettings = paymentAppsettings;
    }

    public async Task SendResponseAsync(Update update)
    {
        var chatId = update.CallbackQuery.From.Id;

        await _telegramBot.SendTextMessageAsync(chatId, "Method not implemented yet");
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Type == UpdateType.CallbackQuery && update.CallbackQuery.Data.Equals(PaymentMethodConstants.Plicio);
    }
}
