using Ardalis.GuardClauses;

using eshopOnTelegram.TelegramBot.Appsettings;

using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.Appsettings;
using eShopOnTelegram.TelegramBot.Commands.Interfaces;
using eShopOnTelegram.TelegramBot.Constants;
using eShopOnTelegram.TelegramBot.Extensions;
using eShopOnTelegram.TelegramBot.Services.Telegram;

namespace eShopOnTelegram.TelegramBot.Commands
{
    public class StartCommand : ITelegramCommand
    {
        private readonly ITelegramBotClient _telegramBot;
        private readonly ILogger<StartCommand> _logger;
        private readonly TelegramAppsettings _telegramAppsettings;
        private readonly BotContentAppsettings _botContentAppsettings;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly PaymentProceedMessageSender _paymentMethodsSender;

        public StartCommand(
            ITelegramBotClient telegramBot,
            ILogger<StartCommand> logger,
            TelegramAppsettings telegramAppsettings,
            BotContentAppsettings botContentAppsettings,
            ICustomerService customerService,
            IOrderService orderService,
            PaymentProceedMessageSender paymentMethodsSender)
        {
            _telegramBot = telegramBot;
            _logger = logger;
            _telegramAppsettings = telegramAppsettings;
            _botContentAppsettings = botContentAppsettings;
            _customerService = customerService;
            _orderService = orderService;
            _paymentMethodsSender = paymentMethodsSender;
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

                    await _telegramBot.SendTextMessageAsync(
                        chatId,
                        _botContentAppsettings.Common.StartError.OrNextIfNullOrEmpty(BotContentDefaultConstants.Common.StartError),
                        ParseMode.Html
                    );
                }

                var keyboardMarkupBuilder = new KeyboardButtonsMarkupBuilder()
                    .AddButtonToCurrentRow(_botContentAppsettings.Common.OpenShopButtonText.OrNextIfNullOrEmpty(BotContentDefaultConstants.Common.OpenShopButtonText), new WebAppInfo() { Url = _telegramAppsettings.WebAppUrl });

                var getOrdersResponse = await _orderService.GetUnpaidOrderByTelegramIdAsync(chatId, CancellationToken.None);
                if (getOrdersResponse.Status == ResponseStatus.Success)
                {
                    var activeOrder = getOrdersResponse.Data;

                    if (activeOrder != null)
                    {
                        keyboardMarkupBuilder
                            .StartNewRow()
                            .AddButtonToCurrentRow(_botContentAppsettings.Order.ShowUnpaidOrder.OrNextIfNullOrEmpty(BotContentDefaultConstants.Order.ShowUnpaidOrder));
                    }
                }

                await _telegramBot.SendTextMessageAsync(
                    chatId,
                    _botContentAppsettings.Common.WelcomeText.OrNextIfNullOrEmpty(BotContentDefaultConstants.Common.WelcomeText),
                    ParseMode.Html,
                    replyMarkup: keyboardMarkupBuilder.Build(resizeKeyboard: true)
                );
            }
            catch (Exception exception)
            {
                var chatId = update.Message.Chat.Id;

                _logger.LogError(exception, exception.Message);
                await _telegramBot.SendCommonErrorMessageAsync(chatId, _botContentAppsettings, CancellationToken.None);
            }
        }

        public bool IsResponsibleForUpdate(Update update)
        {
            return update.Message?.Text?.Contains(CommandConstants.Start) ?? false;
        }
    }
}