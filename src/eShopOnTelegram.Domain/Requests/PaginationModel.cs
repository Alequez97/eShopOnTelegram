namespace eShopOnTelegram.Domain.Requests;

public class PaginationModel
{
    public int? From { get; set; }

    public int? To { get; set; }

    public string? SortPropertyName { get; set; }

    public string? SortBy { get; set; }
}