namespace eShopOnTelegram.RuntimeConfiguration.BotOwnerData.Interfaces;

public interface IBotOwnerDataStore
{
	Task<string?> GetBotOwnerTelegramGroupIdAsync(CancellationToken cancellationToken);

	Task<bool> SaveBotOwnerTelegramGroupIdAsync(string telegramGroupId, CancellationToken cancellationToken);
}
