namespace eShopOnTelegram.Domain.Dto.Orders;

public class OrderDto : DtoBase
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

    public required string Status { get; set; }

    public string? CountryIso2Code { get; set; }

    public string? City { get; set; }

    public string? StreetLine1 { get; set; }

    public string? StreetLine2 { get; set; }

    public string? PostCode { get; set; }
}
