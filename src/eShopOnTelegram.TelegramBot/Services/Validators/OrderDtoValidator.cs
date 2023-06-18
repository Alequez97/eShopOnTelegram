using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Persistence.Entities;

namespace eShopOnTelegram.TelegramBot.Services.Validators;

public class OrderDtoValidator
{
    /// <summary>
    /// Validates that list of orders contains only one with status 'New' or 'InvoiceSent'
    /// </summary>
    /// <returns>Instance of <c>OrderDto</c> if list has only one unpaid item</returns>
    /// <exception cref="Exception">Throws when list has 0 or more than 1 orders with active status</exception>
    public OrderDto ValidateContainsSingleUnpaidOrder(IEnumerable<OrderDto> orders, ILogger logger, bool throwException)
    {
        var customerOrders = orders
            .Where(order => order.Status == OrderStatus.New.ToString() || order.Status == OrderStatus.InvoiceSent.ToString())
            .ToList();

        if (customerOrders.Count > 1)
        {
            var errorMessage = "Error. For every customer should be only one order with status new";

            if (throwException)
            {
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            return null;
        }

        if (customerOrders.Count == 0)
        {
            if (throwException)
            {
                logger.LogError("Error. No active order found");
                throw new Exception($"Error. No active order found");
            }

            return null;
        }

        return customerOrders.First();
    }
}
