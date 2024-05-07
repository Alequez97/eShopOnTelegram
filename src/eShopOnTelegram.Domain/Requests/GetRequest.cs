namespace eShopOnTelegram.Domain.Requests;

public class GetRequest
{
	public int? PageNumber { get; set; }

	public int? ItemsPerPage { get; set; }

	public string? Filter { get; set; }

	[Obsolete("Use PageNumber and ItemsPerPage instead")]
	public string? Range { get; set; }

	public string? Sort { get; set; }

	[Obsolete("Use PaginationModel instead")]
	public PaginationModel PaginationModelObsolete
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

	public PaginationModel PaginationModel
	{
		get
		{
			var paginationModel = new PaginationModel();

			if (PageNumber.HasValue && ItemsPerPage.HasValue)
			{
				paginationModel.From = (PageNumber - 1) * ItemsPerPage + 1;
				paginationModel.To = PageNumber * ItemsPerPage;
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
