using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram;

public class KeyboardButtonsMarkupBuilder
{
    private List<KeyboardButton> _currentKeyboardMarkupRow;
    private readonly List<List<KeyboardButton>> _replyKeyboardMarkup;


    public KeyboardButtonsMarkupBuilder()
    {
        _currentKeyboardMarkupRow = new List<KeyboardButton>();
        _replyKeyboardMarkup = new List<List<KeyboardButton>>();
    }

    public KeyboardButtonsMarkupBuilder AddButtonToCurrentRow(string text, WebAppInfo webAppInfo = null)
    {
        _currentKeyboardMarkupRow.Add(new KeyboardButton(text) { WebApp = webAppInfo });
        return this;
    }

    public KeyboardButtonsMarkupBuilder StartNewRow()
    {
        _replyKeyboardMarkup.Add(_currentKeyboardMarkupRow);
        _currentKeyboardMarkupRow = new List<KeyboardButton>();
        return this;
    }

    public ReplyKeyboardMarkup Build(bool resizeKeyboard = false)
    {
        _replyKeyboardMarkup.Add(_currentKeyboardMarkupRow);
        return new ReplyKeyboardMarkup(_replyKeyboardMarkup)
        {
            ResizeKeyboard = resizeKeyboard
        };
    }
}
