using System.Net;

using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio.Requests;
using eShopOnTelegram.Notifications.Interfaces;
using eShopOnTelegram.Persistence.Entities.Payments;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Interfaces;
using eShopOnTelegram.RuntimeConfiguration.ApplicationContent.Keys;
using eShopOnTelegram.Shop.Api.Constants;

namespace eShopOnTelegram.Shop.Api.Endpoints.Webhooks;

public class PlisioWebhook : EndpointBaseAsync
	.WithRequest<PlisioWebhookRequest>
	.WithActionResult
{
	private readonly ITelegramBotClient _telegramBot;
	private readonly IPaymentService _paymentService;
	private readonly IApplicationContentStore _applicationContentStore;
	private readonly IEnumerable<INotificationSender> _notificationSenders;
	private readonly IWebhookValidator<PlisioWebhookRequest> _validator;
	private readonly ILogger<CoinGateWebhook> _logger;

	public PlisioWebhook(
		ITelegramBotClient telegramBot,
		IPaymentService paymentService,
		IApplicationContentStore applicationContentStore,
		IEnumerable<INotificationSender> notificationSenders,
		IWebhookValidator<PlisioWebhookRequest> validator,
		ILogger<CoinGateWebhook> logger)
	{
		_telegramBot = telegramBot;
		_paymentService = paymentService;
		_applicationContentStore = applicationContentStore;
		_notificationSenders = notificationSenders;
		_validator = validator;
		_logger = logger;
	}

	[HttpPost("/api/webhook/plisio")]
	[SwaggerOperation(Tags = new[] { SwaggerGroup.Webhooks })]
	public override async Task<ActionResult> HandleAsync([FromBody] PlisioWebhookRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var requestBody = await GetRequestBodyAsync();
			var requestIsFromPlisio = await _validator.ValidateAsync(request, requestBody, cancellationToken);

			if (requestIsFromPlisio)
			{
				var confirmPaymentResponse = await _paymentService.ConfirmOrderPaymentAsync(request.OrderNumber, PaymentMethod.Plisio, CancellationToken.None);

				if (confirmPaymentResponse.Status != ResponseStatus.Success)
				{
					return StatusCode((int)HttpStatusCode.ServiceUnavailable);
				}

				await _telegramBot.SendTextMessageAsync(
					confirmPaymentResponse.Data.TelegramUserUID,
					await _applicationContentStore.GetValueAsync(ApplicationContentKey.Payment.SuccessfullPayment, CancellationToken.None)
				);

				foreach (var notificationSender in _notificationSenders)
				{
					await notificationSender.SendOrderReceivedNotificationAsync(request.OrderNumber, cancellationToken);
				}

				return Ok();
			}

			return StatusCode((int)HttpStatusCode.Forbidden);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"[{nameof(CoinGateWebhook)}]: {ex.Message}");

			return StatusCode((int)HttpStatusCode.ServiceUnavailable);
		}
	}

	private async Task<string> GetRequestBodyAsync()
	{
		HttpContext.Request.Body.Position = 0;

		using StreamReader reader = new(HttpContext.Request.Body);
		var bodyAsString = await reader.ReadToEndAsync();

		return bodyAsString;
	}
}
