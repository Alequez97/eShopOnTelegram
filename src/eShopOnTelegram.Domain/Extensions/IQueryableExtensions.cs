using eShopOnTelegram.Domain.Requests;

namespace eShopOnTelegram.Domain.Extensions;

public static class IQueryableExtensions
{
	public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, PaginationModel paginationModel)
	{
		if (!string.IsNullOrWhiteSpace(paginationModel.SortPropertyName))
		{
			var sortingStrategy = string.IsNullOrWhiteSpace(paginationModel.SortBy) ? "ASC" : paginationModel.SortBy;

			query = query
			   .OrderBy($"{paginationModel.SortPropertyName} {sortingStrategy}");
		}

		if (paginationModel.To.HasValue && paginationModel.From.HasValue && paginationModel.To - paginationModel.From > 0)
		{
			query = query
			   .Skip(paginationModel.From.Value)
			   .Take(paginationModel.To.Value - paginationModel.From.Value + 1);
		}

		return query;
	}
}
