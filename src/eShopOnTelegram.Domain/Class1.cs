namespace eShopOnTelegram.Domain
{
    public class Order
    {
        public string OrderNumber { get; set; }

        public string OrderName { get; set; }

        public override string ToString()
        {
            return $"OrderNumber: {OrderNumber}, OrderName: {OrderName}";
        }
    }
}