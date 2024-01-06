using eShopOnTelegram.Shop.Worker.Commands.Interfaces;

namespace eShopOnTelegram.Shop.Worker.Commands.Groups;

public class ChatMemberLeftCommand : ITelegramCommand
{
	public Task SendResponseAsync(Update update)
	{
		return Task.CompletedTask;
	}

	public Task<bool> IsResponsibleForUpdateAsync(Update update)
	{
		return Task.FromResult(update.Message?.Type == MessageType.ChatMemberLeft);
	}
}
