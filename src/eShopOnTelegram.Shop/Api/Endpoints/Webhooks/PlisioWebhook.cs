using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio.Requests;
using eShopOnTelegram.Notifications.Interfaces;
using eShopOnTelegram.Shop.Api.Constants;

namespace eShopOnTelegram.Shop.Api.Endpoints.Webhooks;

public class PlisioWebhook : EndpointBaseAsync
	.WithRequest<PlisioPaymentReceivedWebhookRequest>
	.WithActionResult
{
	private readonly IEnumerable<INotificationSender> _notificationSenders;
	private readonly IWebhookRequestValidator<PlisioPaymentReceivedWebhookRequest> _validator;

	public PlisioWebhook(
		IEnumerable<INotificationSender> notificationSenders,
		IWebhookRequestValidator<PlisioPaymentReceivedWebhookRequest> validator)
	{
		_notificationSenders = notificationSenders;
		_validator = validator;
	}

	[HttpPost("/api/webhook/plisio")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Webhooks })]
	public override async Task<ActionResult> HandleAsync([FromBody] PlisioPaymentReceivedWebhookRequest request, CancellationToken cancellationToken)
	{
		var requestIsFromPlisio = _validator.Validate(request);

		if (requestIsFromPlisio)
		{
			// TODO: Check payment status

			foreach (var notificationSender in _notificationSenders)
			{
				await notificationSender.SendOrderReceivedNotificationAsync(request.OrderNumber, cancellationToken);
			}

			// TODO: Update order status

			return Ok();
		}

		return BadRequest();
	}
}
