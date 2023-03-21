namespace eShopOnTelegram.Persistence.Constraints;

public static class CustomerConstraints
{
    public static ModelBuilder AddCustomerConstraints(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Customer>()
            .HasIndex(customer => customer.Username)
            .IsUnique();

        modelBuilder
            .Entity<Customer>()
            .HasIndex(customer => customer.TelegramId)
            .IsUnique();

        return modelBuilder;
    }
}