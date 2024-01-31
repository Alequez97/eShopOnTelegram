namespace eShopOnTelegram.ExternalServices.Interfaces;

public interface IWebhookValidator<T>
{
	bool Validate(T request, string requestBody);
}
