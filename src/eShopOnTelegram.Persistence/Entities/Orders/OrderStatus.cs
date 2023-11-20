namespace eShopOnTelegram.Persistence.Entities.Orders;

public enum OrderStatus
{
    Canceled = -1,
    New = 0,
    AwaitingPayment = 1,
    Paid = 2,
    Delivering = 3,
    Delivered = 4,
    Finished = 5
}
