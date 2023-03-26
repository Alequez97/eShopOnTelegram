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

        if (paginationModel.To - paginationModel.From > 0)
        {
            query = query
               .Skip(paginationModel.From)
               .Take(paginationModel.To - paginationModel.From + 1);
        }

        return query;
    }
}