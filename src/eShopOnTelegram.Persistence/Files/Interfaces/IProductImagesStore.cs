namespace eShopOnTelegram.Persistence.Files.Interfaces;

public interface IProductImagesStore
{
    public Task<string> SaveAsync(byte[] file, string fileName, CancellationToken cancellationToken);

    public Task DeleteAsync(string fileName);
}
