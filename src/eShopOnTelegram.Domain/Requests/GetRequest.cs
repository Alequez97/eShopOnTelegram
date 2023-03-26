namespace eShopOnTelegram.Domain.Requests;

public class GetRequest
{
    public string? Filter { get; set; }

    public string? Range { get; set; }

    public string? Sort { get; set; }

    public PaginationModel? PaginationModel
    {
        get
        {
            var paginationModel = new PaginationModel();

            if (!string.IsNullOrWhiteSpace(Range))
            {
                var rangeAsArray = Range
                .Replace('[', ' ')
                .Replace(']', ' ')
                .Replace(" ", "")
                .Split(',');

                int.TryParse(rangeAsArray[0], out var from);
                int.TryParse(rangeAsArray[1], out var to);

                paginationModel.From = from;
                paginationModel.To = to;
            }

            if (!string.IsNullOrWhiteSpace(Sort))
            {
                var sortAsArray = Sort
                .Replace('[', ' ')
                .Replace(']', ' ')
                .Replace('"', ' ')
                .Replace(" ", "")
                .Split(',');

                paginationModel.SortPropertyName = sortAsArray[0];
                paginationModel.SortBy = sortAsArray.Length > 1 ? sortAsArray[1] : string.Empty;
            }

            return paginationModel;
        }
    }
}