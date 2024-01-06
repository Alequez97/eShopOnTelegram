namespace eShopOnTelegram.Persistence.Context;

public class EShopOnTelegramDbContextFactory : IDesignTimeDbContextFactory<EShopOnTelegramDbContext>
{
	public EShopOnTelegramDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<EShopOnTelegramDbContext>();
		optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=eShopOnTelegramDb;Trusted_Connection=True");

		return new EShopOnTelegramDbContext(optionsBuilder.Options);
	}
}
