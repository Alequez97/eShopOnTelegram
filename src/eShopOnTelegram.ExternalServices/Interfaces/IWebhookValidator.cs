namespace eShopOnTelegram.ExternalServices.Interfaces;

public interface IWebhookValidator<T>
{
	Task<bool> ValidateAsync(T request, string requestBody = null, CancellationToken cancellationToken = default);
}
