namespace eShopOnTelegram.Persistence.Context;

public class EShopOnTelegramDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Basket> Baskets { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public EShopOnTelegramDbContext(DbContextOptions<EShopOnTelegramDbContext> options)
    : base(options)
    {

    }
}
