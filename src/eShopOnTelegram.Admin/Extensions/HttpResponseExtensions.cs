using eShopOnTelegram.Domain.Requests;

namespace eShopOnTelegram.Admin.Extensions;

public static class HttpResponseExtensions
{
	public static void AddPaginationHeaders(this HttpResponse httpResponse, PaginationModel paginationModel, int totalItemsCount)
	{
		httpResponse.Headers.TryAdd("Content-Range", $"customers {paginationModel.From}-{paginationModel.To}/{totalItemsCount}");
		httpResponse.Headers.TryAdd("Access-Control-Expose-Headers", "Content-Range");
	}

	public static void AddMockPaginationHeaders(this HttpResponse httpResponse)
	{
		httpResponse.Headers.TryAdd("Content-Range", $"customers 1-10/10");
		httpResponse.Headers.TryAdd("Access-Control-Expose-Headers", "Content-Range");
	}
}
