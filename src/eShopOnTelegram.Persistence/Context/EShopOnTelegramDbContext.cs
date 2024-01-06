using eShopOnTelegram.Persistence.Entities.Orders;
using eShopOnTelegram.Persistence.Entities.Products;
using eShopOnTelegram.Persistence.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace eShopOnTelegram.Persistence.Context;

public class EShopOnTelegramDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
	public DbSet<Customer> Customers { get; set; }

	public DbSet<Product> Products { get; set; }

	public DbSet<ProductAttribute> ProductAttributes { get; set; }

	public DbSet<ProductCategory> ProductCategories { get; set; }

	public DbSet<Order> Orders { get; set; }

	public DbSet<CartItem> CartItems { get; set; }

	public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

	public EShopOnTelegramDbContext(DbContextOptions<EShopOnTelegramDbContext> options)
	: base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<User>(b =>
			b.HasMany(e => e.Claims)
			.WithOne()
			.HasForeignKey(uc => uc.UserId)
			.IsRequired()
		);

		var adminUser = new User
		{
			Id = 1,
			UserName = "admin",
			NormalizedUserName = "ADMIN",
			SecurityStamp = Guid.NewGuid().ToString()
		};

		PasswordHasher<User> ph = new();
		adminUser.PasswordHash = ph.HashPassword(adminUser, "1234");

		builder.Entity<User>().HasData(adminUser);

		var claim = new IdentityUserClaim<long>()
		{
			Id = 1,
			ClaimType = "Role",
			ClaimValue = "superadmin",
			UserId = 1
		};
		builder.Entity<IdentityUserClaim<long>>().HasData(claim);
	}
}
