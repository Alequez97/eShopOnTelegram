using eShopOnTelegram.TelegramBot.Commands;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Services.Telegram;

public class CommandResolver
{
    private readonly IEnumerable<ITelegramCommand> _commands;
    private readonly UnknownCommand _unknownCommand;

    public CommandResolver(IEnumerable<ITelegramCommand> commands, UnknownCommand unknownCommand)
    {
        _commands = commands;
        _unknownCommand = unknownCommand;
    }

    public async Task<ITelegramCommand> ResolveAsync(Update update)
    {
        foreach (var command in _commands)
        {
            if (await command.IsResponsibleForUpdateAsync(update))
            {
                return command;
            }
        }

        return _unknownCommand;
    }
}