using Ardalis.GuardClauses;

using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Interfaces;
using eShopOnTelegram.TelegramBot.Services;

using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.TelegramBot.Commands
{
    public class StartCommand : ITelegramCommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<StartCommand> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICustomerService _customerService;

        public StartCommand(
            ITelegramBotClient telegramBotClient,
            ILogger<StartCommand> logger,
            IConfiguration configuration,
            ICustomerService customerService)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _configuration = configuration;
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

                var createCustomerResponse = await _customerService.CreateUserIfNotPresentAsync(createCustomerRequest);
                var chatId = update.Message.Chat.Id;

                if (createCustomerResponse.Status != Domain.Responses.ResponseStatus.Success)
                {
                    _logger.LogError("Unable to persist new customer.");
                    await _telegramBotClient.SendTextMessageAsync(
                        chatId,
                        $"Unable to register at this moment. Sorry!!!",
                        ParseMode.MarkdownV2
                    );
                }

                var keyboardMarkup = new TelegramKeyboardButtonsMarkupBuilder()
                    .AddButtonToCurrentRow(ButtonConstants.OpenShop, new WebAppInfo() { Url = _configuration["Telegram:WebAppUrl"] })
                    .Build(resizeKeyboard: true);

                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    $"Welcome to eShopOnTelegram",
                    ParseMode.MarkdownV2,
                    replyMarkup: keyboardMarkup
                );
            }
            catch (Exception ex)
            {

            }
        }

        public bool IsResponsibleForUpdate(Update update)
        {
            return update.Message?.Text?.Contains(CommandConstants.Start) ?? false;
        }
    }
}