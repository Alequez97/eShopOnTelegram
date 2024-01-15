using Telegram.Bot.Types.ReplyMarkups;

namespace eShopOnTelegram.Shop.Worker.Services.Telegram.Buttons.Keyboard;

public class KeyboardButtonsLayoutBuilder
{
	private List<KeyboardButton> _currentKeyboardMarkupRow;
	private readonly List<List<KeyboardButton>> _replyKeyboardMarkup;


	public KeyboardButtonsLayoutBuilder()
	{
		_currentKeyboardMarkupRow = new List<KeyboardButton>();
		_replyKeyboardMarkup = new List<List<KeyboardButton>>();
	}

	public KeyboardButtonsLayoutBuilder AddButtonToCurrentRow(string text, WebAppInfo webAppInfo = null)
	{
		_currentKeyboardMarkupRow.Add(new KeyboardButton(text) { WebApp = webAppInfo });
		return this;
	}

	public KeyboardButtonsLayoutBuilder StartNewRow()
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
