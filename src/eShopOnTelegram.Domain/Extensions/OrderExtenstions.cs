using System.Runtime.CompilerServices;

using eShopOnTelegram.Domain.Dto.Orders;

namespace eShopOnTelegram.Domain.Extensions;

public static class OrderExtenstions
{
    public static OrderDto ToOrderDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            TelegramUserUID = order.Customer.TelegramUserUID,
            Username = order.Customer.Username,
            FirstName = order.Customer.FirstName,
            LastName = order.Customer.LastName,
            CartItems = order.CartItems.Select(cartItem => new CartItemDto()
            {
                ProductId = cartItem.ProductId,
                Name = cartItem.Product.Name,
                CategoryName = cartItem.Product.Category.Name,
                OriginalPrice = cartItem.Product.OriginalPrice,
                PriceWithDiscount = cartItem.Product.PriceWithDiscount,
                QuantityLeft = cartItem.Product.QuantityLeft, // probably we dont need this
                ImageName = cartItem.Product.ImageName,
                Quantity = cartItem.Quantity
            }).ToList(),
            CreationDate = order.CreationDate,
            PaymentDate = order.PaymentDate,
            Status = order.Status.ToString(),
            CountryIso2Code = order.CountryIso2Code,
            City = order.City,
            StreetLine1 = order.StreetLine1,
            StreetLine2 = order.StreetLine2,
            PostCode = order.PostCode,
        };
    }
}
