using eShopOnTelegram.Shop.Worker.Commands.Interfaces;

namespace eShopOnTelegram.Shop.Worker.Commands.Messages;

public class MessagePinnedCommand : ITelegramCommand
{
	public Task SendResponseAsync(Update update)
	{
		return Task.CompletedTask;
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Message?.Type == MessageType.MessagePinned);
	}
}
