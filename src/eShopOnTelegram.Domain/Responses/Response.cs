namespace eShopOnTelegram.Domain.Responses;

public abstract class Response
{
    public required ResponseStatus Status { get; set; }

    public string? Message { get; set; }

    public List<string>? ValidationErrors { get; set; }
}

public class ActionResponse : Response
{
    public long Id { get; set; }
}

public class Response<T> : Response where T : class
{
    public T Data { get; set; }

    public int TotalItemsInDatabase { get; set; }
}
