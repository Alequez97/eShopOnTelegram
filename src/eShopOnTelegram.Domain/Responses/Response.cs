namespace eShopOnTelegram.Domain.Responses;

public class Response
{
    public long Id { get; set; }

    public ResponseStatus Status { get; set; }

    public string? Message { get; set; }

    public List<string>? ValidationErrors { get; set; }
}

public class Response<T> : Response where T : class
{
    public T Data { get; set; }

    public int TotalItemsInDatabase { get; set; }
}