namespace eShopOnTelegram.Shop.Worker.Exceptions;

public class InvoiceSenderNotFoundException : Exception
{
	/// <summary>
	/// As argument accepts payment method as string
	/// </summary>
	/// <param name="paymentMethod"></param>
	public InvoiceSenderNotFoundException(string paymentMethod) : base($"Invoice sender not found for {paymentMethod} payment method")
	{
		
	}
}
