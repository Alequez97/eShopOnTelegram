namespace eShopOnTelegram.ExternalServices.Interfaces;

public interface IWebhookRequestValidator<T>
{
	bool Validate(T request);
}
