﻿using eShopOnTelegram.ExternalServices.Services.Plisio.Responses;

using Refit;

namespace eShopOnTelegram.ExternalServices.Services.Plisio;

public interface IPlisioClient
{
	[Get("/invoices/new?source_currency={sourceCurrency}&source_amount={sourceAmount}&order_number={orderNumber}&currency={currency}&api_key={apiKey}&callback_url={callbackUrl}&order_name={orderName}")]
	public Task<CreatePlisioInvoiceResponse> CreateInvoiceAsync(
		string apiKey,
		string sourceCurrency,
		int sourceAmount,
		string orderNumber,
		string currency,
		string callbackUrl,
		string orderName
	);
}
