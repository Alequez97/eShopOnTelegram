using Microsoft.AspNetCore.Http;

namespace eShopOnTelegram.Persistence.Files.Interfaces;

public interface IProductImagesStore
{
    public Task<string> SaveAsync(IFormFile image, CancellationToken cancellationToken);

    public Task DeleteAsync(string id);
}
