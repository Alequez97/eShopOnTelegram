namespace eShopOnTelegram.Persistence.Constraints;

public static class OrderConstratints
{
    public static ModelBuilder AddOrderConstraints(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Order>()
            .HasIndex(order => order.OrderNumber)
            .IsUnique();

        return modelBuilder;
    }
}
