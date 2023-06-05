using eShopOnTelegram.TelegramBot.Commands;

using TelegramBot.Commands.Interfaces;

namespace TelegramBot.Services.Telegram
{
    public class CommandResolver
    {
        private readonly IEnumerable<ITelegramCommand> _commands;
        private readonly UnknownCommand _unknownCommand;

        public CommandResolver(IEnumerable<ITelegramCommand> commands, UnknownCommand unknownCommand)
        {
            _commands = commands;
            _unknownCommand = unknownCommand;
        }

        public ITelegramCommand Resolve(Update update)
        {
            var command = _commands.FirstOrDefault(command => command.IsResponsibleForUpdate(update));

            if (command == null)
            {
                return _unknownCommand;
            }

            return command;
        }
    }
}