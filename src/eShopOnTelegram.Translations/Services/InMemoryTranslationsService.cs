using eShopOnTelegram.Translations.Constants;
using eShopOnTelegram.Translations.Interfaces;

namespace eShopOnTelegram.Translations.Services;
public class InMemoryTranslationsService : ITranslationsService
{
	private static readonly Dictionary<string, Dictionary<string, string>> _translations = new()
	{
		{
			Language.EN,
			new()
			{
				// Values, that can be overriden later by shop administrator
				{ TranslationsKeys.WelcomeToOurShop, "Welcome to our shop👋 <tg-spoiler>Use this as welcome message of your shop and give customer more information what you offer, in which regions you work, etc.</tg-spoiler>"},
				{ TranslationsKeys.UnknownCommand, "We glad to help you with good products and fast delivery. Click button on keyboard to open shop and look for our products"},
				{ TranslationsKeys.DefaultErrorMessage, "Something went wrong. Please try again later or contact our support"},
				{ TranslationsKeys.ThankYouForPurchase, "Thank you for your purchase"},
				{ TranslationsKeys.PaymentThroughSeller, "Please contact support team to discuss further payment details"},

				// Constants, that can't be changed by shop administrator
				{ TranslationsKeys.Error_TryAgainLater, "Error. Try again later"},
				{ TranslationsKeys.NoAvailableProducts, "No available products at this moment" },
				{ TranslationsKeys.AllCategories, "All categories" },
				{ TranslationsKeys.Continue, "Continue" },
				{ TranslationsKeys.ProceedToPayment, "Proceed to payment" },
				{ TranslationsKeys.UnpaidOrderNotFound, "Unpaid order not found. Visit our shop to create new order" },
				{ TranslationsKeys.ErrorDuringOrderCreation, "Error during order creation. Try again later" },
				{ TranslationsKeys.IncorrectInvoiceChoosen, "You chose an incorrect invoice to pay for. Chose to pay for invoice, that is last in all created" },
				{ TranslationsKeys.OrderNumber, "Order number" },
				{ TranslationsKeys.OrderSummary, "Your order summary" },
				{ TranslationsKeys.UnableToGetShippingAddress, "Unable to get shipping address. Please contact bot support team and precise where you want to receive your order delivery" },
				{ TranslationsKeys.ChoosePaymentMethod, "Please select a payment method" },
				{ TranslationsKeys.PayWithBankCard, "Pay with bank card" },
				{ TranslationsKeys.OrderAlreadyPaidOrExpired, "You already paid for this order, or time to accomplish expired. Try again to create a new order" },
				{ TranslationsKeys.InvoiceGenerationFailedErrorMessage, "Error during invoice generation. Try again later" },
				{ TranslationsKeys.TotalPrice, "Total price" },
				{ TranslationsKeys.OpenShop, "🛒 Open shop 🛒" },
				{ TranslationsKeys.Category, "Category" },
				{ TranslationsKeys.SelectCategory, "Select category" },
				{ TranslationsKeys.BackToCategories, "⬅️ Back to categories" },
				{ TranslationsKeys.BackToProducts, "⬅️ Back to products" },
				{ TranslationsKeys.Product, "Product" },
				{ TranslationsKeys.ShowUnpaidOrder, "Show unpaid order" },
				{ TranslationsKeys.PaymentMethodAlreadySelected, "Payment method already selected. Pay for invoice you received or create new order" },
				{ TranslationsKeys.NoEnabledPaymentMethods, "No enabled payment methods" },
				{ TranslationsKeys.InvoiceReceived, "Please receive your invoice" },
			}
		},
		{
			Language.RU,
			new ()
			{
				// Values, that can be overriden later by shop administrator
				{ TranslationsKeys.WelcomeToOurShop, "Добро пожаловать в наш магазин👋 <tg-spoiler>Используйте это сообщение для того, чтобы сообщить пользователям о том, в каком регионе вы работаете, какой товар предлагаете и т.д.</tg-spoiler>"},
				{ TranslationsKeys.UnknownCommand, "Мы всегда рады помочь нашим клиентам быстрой доставкой и качественными продуктами. Нажмите кнопку внизу и перейдите в каталог товаров"},
				{ TranslationsKeys.DefaultErrorMessage, "Произошла ошибка. Повторите попытку позже или свяжитесь с нашей командой поддержки"},
				{ TranslationsKeys.ThankYouForPurchase, "Благодарим, ваш заказ получен!"},
				{ TranslationsKeys.PaymentThroughSeller, "Пожалуйста, свяжитесь с командой поддержки, чтобы обсудить дальнейшие детали оплаты"},

				// Constants, that can't be changed by shop administrator
				{ TranslationsKeys.Error_TryAgainLater, "Ошибка. Повторите попытку позже"},
				{ TranslationsKeys.NoAvailableProducts, "На данный момент нет доступных товаров" },
				{ TranslationsKeys.AllCategories, "Все категории" },
				{ TranslationsKeys.Continue, "Продолжить" },
				{ TranslationsKeys.ProceedToPayment, "Перейти к оплате" },
				{ TranslationsKeys.UnpaidOrderNotFound, "Неоплаченный заказ не найден. Перейдите в наш магазин, чтобы создать новый заказ" },
				{ TranslationsKeys.ErrorDuringOrderCreation, "Ошибка при создании заказа. Повторите попытку позже" },
				{ TranslationsKeys.IncorrectInvoiceChoosen, "Вы выбрали неверную квитанцию для оплаты. Оплатите последнюю квитанцию" },
				{ TranslationsKeys.OrderNumber, "Заказ номер" },
				{ TranslationsKeys.OrderSummary, "Ваш заказ" },
				{ TranslationsKeys.UnableToGetShippingAddress, "Возникла ошибка при проверке адреса доставки. Сважитесь с командой поддержки и уточните куда вы хотите получить доставку своего заказа" },
				{ TranslationsKeys.ChoosePaymentMethod, "Выберете способ оплаты" },
				{ TranslationsKeys.PayWithBankCard, "Оплата банковской картой" },
				{ TranslationsKeys.OrderAlreadyPaidOrExpired, "Заказ уже был оплачен или время для подтверждения закончилось. Перейдите в магазин и создайте новый заказ" },
				{ TranslationsKeys.InvoiceGenerationFailedErrorMessage, "Возникла ошибка при отправке квитанции об оплате. Попробуйте повторить попытку позже" },
				{ TranslationsKeys.TotalPrice, "Общая сумма" },
				{ TranslationsKeys.OpenShop, "🛒 Открыть магазин 🛒" },
				{ TranslationsKeys.Category, "Категория" },
				{ TranslationsKeys.SelectCategory, "Выберите категорию" },
				{ TranslationsKeys.BackToCategories, "⬅️ Обратно к категориям" },
				{ TranslationsKeys.BackToProducts, "⬅️ Обратно к продуктам" },
				{ TranslationsKeys.Product, "Товар" },
				{ TranslationsKeys.ShowUnpaidOrder, "Посмотреть неоплаченный заказ" },
				{ TranslationsKeys.PaymentMethodAlreadySelected, "Способ оплаты уже выбран. Оплатите полученную квитанцию или создайте новый заказ" },
				{ TranslationsKeys.NoEnabledPaymentMethods, "Нет доступных способов оплаты" },
				{ TranslationsKeys.InvoiceReceived, "Ваша квитанция для оплаты" },
			}
		}
	};

	public async Task<string> TranslateAsync(string language, string translationsKey, CancellationToken cancellationToken)
	{
		var translationsForLanguage = _translations[language];

		if (translationsForLanguage.TryGetValue(translationsKey, out var translation))
		{
			return await Task.FromResult(translation);
		}

		throw new NotSupportedException($"Missing {translationsKey} translations key for {language} language");
	}

	public bool IsLanguageSupported(string language)
	{
		return _translations.ContainsKey(language);
	}
}
