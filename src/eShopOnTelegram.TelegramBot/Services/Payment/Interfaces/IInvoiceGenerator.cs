using eShopOnTelegram.Domain.Requests.Orders;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Services.Payment.Interfaces;

public interface IInvoiceGenerator
{
    InlineKeyboardButton GenerateInvoiceButton(IEnumerable<CreateCartItemRequest> cartItems);
}
