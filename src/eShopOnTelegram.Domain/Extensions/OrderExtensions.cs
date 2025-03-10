﻿using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Dto.ProductAttributes;
using eShopOnTelegram.Persistence.Entities.Orders;

namespace eShopOnTelegram.Domain.Extensions;

public static class OrderExtensions
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
				ProductAttribute = new ProductAttributeDto()
				{
					Id = cartItem.ProductAttribute.Id,
					Color = cartItem.ProductAttribute.Color,
					Size = cartItem.ProductAttribute.Size,
					ProductName = cartItem.ProductAttribute.ProductName,
					ProductCategoryName = cartItem.ProductAttribute.ProductCategoryName,
					OriginalPrice = cartItem.ProductAttribute.OriginalPrice,
					PriceWithDiscount = cartItem.ProductAttribute.PriceWithDiscount,
					QuantityLeft = cartItem.ProductAttribute.QuantityLeft,
				},
				Quantity = cartItem.Quantity
			}).ToList(),
			CreationDate = order.CreationDate,
			PaymentDate = order.PaymentDetails.PaymentDate,
			Status = order.Status.ToString(),
			PaymentStatus = order.PaymentDetails.PaymentStatus.ToString(),
			PaymentMethod = order.PaymentDetails.PaymentMethod.ToString(),
			InvoiceUrl = order.PaymentDetails.InvoiceUrl,
			Country = order.Country,
			City = order.City,
			StreetLine1 = order.StreetLine1,
			StreetLine2 = order.StreetLine2,
			PostCode = order.PostCode,
		};
	}
}
