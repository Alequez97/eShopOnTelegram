using System.ComponentModel.DataAnnotations;

namespace eShopOnTelegram.Domain.Dto.Orders;

public class CartItemDto
{
    public required long ProductId { get; set; }
    
    public string Name { get; set; }

    public string CategoryName { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal? PriceWithDiscount { get; set; }

    [MaxLength(200)]
    public string? ImageName { get; set; }
    
    public required int Quantity { get; set; }

    public decimal TotalPrice => (PriceWithDiscount ?? OriginalPrice) * Quantity;
}
