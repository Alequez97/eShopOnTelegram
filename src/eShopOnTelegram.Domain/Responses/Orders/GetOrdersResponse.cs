using System.ComponentModel.DataAnnotations;

namespace eShopOnTelegram.Domain.Responses.Orders;

public class GetOrdersResponse : Response
{
    public required string OrderNumber { get; set; }

    // Customer
    public required long CustomerId { get; set; }
    public required long TelegramUserUID { get; set; }

    public string? Username { get; set; }

    public required string FirstName { get; set; }

    public string? LastName { get; set; }
    //

    public required IList<CartItemDto> CartItems { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? PaymentDate { get; set; }

    public required OrderStatus Status { get; set; }

    public string? CountryIso2Code { get; set; }

    public string? City { get; set; }

    public string? StreetLine1 { get; set; }

    public string? StreetLine2 { get; set; }

    public string? PostCode { get; set; }
}

public class CartItemDto
{
    public required long ProductId { get; set; }
    // Product
    public string Name { get; set; }

    //Category
    public string CategoryName { get; set; }
    //

    public decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    public int QuantityLeft { get; set; }

    [MaxLength(200)]
    public string? ImageName { get; set; }
    //
    public required int Quantity { get; set; }
}