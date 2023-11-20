namespace eShopOnTelegram.Persistence.Entities.Orders;

public enum OrderPaymentStatus
{
    // ? Refunded
    // ? PaymentFailure
    None = 0,
    InvoiceSent = 1,
    PaymentSuccessful = 2,
}