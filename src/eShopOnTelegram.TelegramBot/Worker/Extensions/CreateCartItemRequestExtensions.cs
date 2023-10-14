using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Responses;

using Telegram.Bot.Types.Payments;

namespace eShopOnTelegram.TelegramBot.Worker.Extensions;

public static class CreateCartItemRequestExtensions
{
    public static async Task<IEnumerable<LabeledPrice>> GetPaymentLabeledPricesAsync(this IEnumerable<CartItemDto> cartItems, IProductAttributeService productAttributeService, CancellationToken cancellationToken)
    {
        var labeledPrices = new List<LabeledPrice>();

        foreach (var cartItem in cartItems)
        {
            var response = await productAttributeService.GetAsync(cartItem.ProductAttribute.Id, cancellationToken);

            if (response.Status == ResponseStatus.Success)
            {
                var existingProductAttribute = response.Data;

                labeledPrices.Add(
                    new LabeledPrice(cartItem.GetFormattedMessage(' '),
                    (int)((existingProductAttribute.PriceWithDiscount ?? existingProductAttribute.OriginalPrice) * cartItem.Quantity * 100)));
            }
        }

        return labeledPrices;
    }
}
