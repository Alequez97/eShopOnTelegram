namespace eShopOnTelegram.Persistence.Entities.Orders;

public enum PaymentStatus
{
    // ? Refunded
    // ? PaymentFailure
    None = 0,
    InvoiceSent = 1,
    PaymentSuccessful = 2,
}
