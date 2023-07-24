using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;

using Telegram.Bot.Types.Payments;

namespace eShopOnTelegram.TelegramBot.Worker.Extensions;

public static class CreateCartItemRequestExtensions
{
    public static async Task<IEnumerable<LabeledPrice>> GetPaymentLabeledPricesAsync(this IEnumerable<CartItemDto> cartItems, IProductService productService, CancellationToken cancellationToken)
    {
        var labeledPrices = new List<LabeledPrice>();

        foreach (var cartItem in cartItems)
        {
            var response = await productService.GetAsync(cartItem.ProductId, cancellationToken);

            if (response.Status == ResponseStatus.Success)
            {
                var existingProduct = response.Data;

                labeledPrices.Add(
                    new LabeledPrice($"{existingProduct.Name} ({existingProduct.PriceWithDiscount ?? existingProduct.OriginalPrice}) x{cartItem.Quantity}",
                    (int)((existingProduct.PriceWithDiscount ?? existingProduct.OriginalPrice) * cartItem.Quantity * 100)));
            }
        }

        return labeledPrices;
    }
}
