using Ardalis.GuardClauses;

using eshopOnTelegram.TelegramBot.Appsettings;

using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Services.Telegram;

namespace eShopOnTelegram.TelegramBot.Commands
{
    public class StartCommand : ITelegramCommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<StartCommand> _logger;
        private readonly TelegramAppsettings _telegramAppsettings;
        private readonly BotContentAppsettings _botContentAppsettings;
        private readonly ICustomerService _customerService;

        public StartCommand(
            ITelegramBotClient telegramBotClient,
            ILogger<StartCommand> logger,
            TelegramAppsettings telegramAppsettings,
            BotContentAppsettings botContentAppsettings,
            ICustomerService customerService)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _telegramAppsettings = telegramAppsettings;
            _botContentAppsettings = botContentAppsettings;
            _customerService = customerService;
        }

        public async Task SendResponseAsync(Update update)
        {
            try
            {
                Guard.Against.Null(update.Message);
                Guard.Against.Null(update.Message.From);

                var createCustomerRequest = new CreateCustomerRequest()
                {
                    TelegramUserUID = update.Message.From.Id,
                    Username = update.Message.From.Username,
                    FirstName = update.Message.From.FirstName,
                    LastName = update.Message.From.LastName
                };

                var createCustomerResponse = await _customerService.CreateIfNotPresentAsync(createCustomerRequest);
                var chatId = update.Message.Chat.Id;

                if (createCustomerResponse.Status != ResponseStatus.Success)
                {
                    _logger.LogError("Unable to persist new customer.");
                    await _telegramBotClient.SendTextMessageAsync(
                        chatId,
                        $"Unable to register at this moment. Sorry!!!",
                        ParseMode.MarkdownV2
                    );
                }

                var keyboardMarkup = new KeyboardButtonsMarkupBuilder()
                    .AddButtonToCurrentRow(_botContentAppsettings.OpenShopButtonText, new WebAppInfo() { Url = _telegramAppsettings.WebAppUrl })
                    .Build(resizeKeyboard: true);

                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    _botContentAppsettings.WelcomeText,
                    ParseMode.MarkdownV2,
                    replyMarkup: keyboardMarkup
                );
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }

        public bool IsResponsibleForUpdate(Update update)
        {
            return update.Message?.Text?.Contains(CommandConstants.Start) ?? false;
        }
    }
}