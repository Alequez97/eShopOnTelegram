using eShopOnTelegram.ExternalServices.Interfaces;
using eShopOnTelegram.ExternalServices.Services.Plisio.Requests;
using eShopOnTelegram.TelegramBot.Api.Constants;

namespace eShopOnTelegram.TelegramBot.Api.Endpoints.Webhooks;

public class PlisioWebhook : EndpointBaseAsync
    .WithRequest<PlisioPaymentReceivedWebhookRequest>
    .WithActionResult
{
    private readonly ITelegramBotClient _telegramBot;
    private readonly IWebhookRequestValidator<PlisioPaymentReceivedWebhookRequest> _validator;

    public PlisioWebhook(
        ITelegramBotClient telegramBot,
        IWebhookRequestValidator<PlisioPaymentReceivedWebhookRequest> validator)
    {
        _telegramBot = telegramBot;
        _validator = validator;
    }

    [HttpPost("/api/webhook/plisio")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Webhooks })]
    public override async Task<ActionResult> HandleAsync([FromBody] PlisioPaymentReceivedWebhookRequest request, CancellationToken cancellationToken = default)
    {
        var requestIsFromPlisio = _validator.Validate(request);

        if (requestIsFromPlisio)
        {
            // TODO: Update order status, send notification to group
            return Ok();
        }

        return BadRequest();
    }
}
