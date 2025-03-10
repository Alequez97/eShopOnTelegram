﻿namespace eShopOnTelegram.Domain.Requests.Orders;

public class UpdateDeliveryAddressRequest
{
	public string? Country { get; set; }

	public string? City { get; set; }

	public string? StreetLine1 { get; set; }

	public string? StreetLine2 { get; set; }

	public string? PostCode { get; set; }
}
