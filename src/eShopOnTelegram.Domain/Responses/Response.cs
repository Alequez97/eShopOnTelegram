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

	[Obsolete("Use metadata instead")]
	public int TotalItemsInDatabase { get; set; }

	public ResponseMetadata? Metadata { get; set; }
}

public class ResponseMetadata
{
	public int PageNumber { get; set; }

	public required int ItemsPerPage { get; set; }

	public int TotalPages
	{
		get
		{
			if (ItemsPerPage <= 0)
			{
				return 1;
			}

			return (TotalItemsInDatabase - 1) / ItemsPerPage + 1;
		}
	}

	public required int TotalItemsInDatabase { get; set; }
}
