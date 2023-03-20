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

        public StartCommand(
            ITelegramBotClient telegramBotClient,
            ILogger<StartCommand> logger,
            IConfiguration configuration)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendResponseAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;

            //var createCustomerRequest = new CreateCustomerRequest()
            //{
            //    Username = update.Message.From.Username ?? update.Message.From.FirstName,
            //    FirstName = update.Message.From.FirstName,
            //    LastName = update.Message.From.LastName,
            //    CustomerProvider = CustomerProvider.Telegram,
            //    IdInCustomerProviderSystem = update.Message.From.Id.ToString(),
            //};
            //var response = await _profileService.CreateAsync(createCustomerRequest);

            //if (!response.IsSuccess)
            //{
            //    await _telegramBotClient.SendTextMessageAsync(
            //        chatId,
            //        $"Unable to register at this moment. Sorry!!!",
            //        ParseMode.MarkdownV2
            //    );
            //    //TODO: Log, that customer profile was not stored in database

            //    return;
            //}

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

        public bool IsResponsibleForUpdate(Update update)
        {
            return update.Message?.Text?.Contains(CommandConstants.Start) ?? false;
        }
    }
}