using eShopOnTelegram.TelegramBot.Worker.Commands;
using eShopOnTelegram.TelegramBot.Worker.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Worker.Exceptions;

namespace eShopOnTelegram.TelegramBot.Worker.Services.Telegram;

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
        var responsibleCommands = _commands.Where(command => command.IsResponsibleForUpdateAsync(update).Result).ToList();

        if (responsibleCommands.Count > 1)
        {
            var responsibleForUpdateTypesAsString = string.Join(", ", responsibleCommands.Select(command => command.GetType().Name));
            throw new MoreThanOneCommandIsResponsibleForUpdateException($"More than one command found as responsible for incoming update. List of commands that match same update: {responsibleForUpdateTypesAsString}");
        }

        if (responsibleCommands.Count == 0)
        {
            return _unknownCommand;
        }

        return await Task.FromResult(responsibleCommands.First());
    }
}