using Newtonsoft.Json;

namespace eShopOnTelegram.ApplicationContent.Models;

public class ApplicationContentModel
{
    public required TelegramBotContent TelegramBot { get; set; }

    public required OrderContent Order { get; set; }

    public required PaymentContent Payment { get; set; }
}

public class TelegramBotContent
{
    public required string StartError { get; set; }
    public required string WelcomeText { get; set; }
    public required string OpenShopButtonText { get; set; }
    public required string UnknownCommandText { get; set; }
    public required string DefaultErrorMessage { get; set; }
}

public class OrderContent
{
    public required string ShowUnpaidOrder { get; set; }
    public required string NoUnpaidOrderFound { get; set; }
    public required string AlreadyPaidOrExpired { get; set; }
    public required string CreateErrorMessage { get; set; }
    public required string InvoiceGenerationFailedErrorMessage { get; set; }
    public required string OrderNumberTitle { get; set; }
    public required string OrderSummaryTitle { get; set; }
    public required string TotalPriceTitle { get; set; }
}

public class PaymentContent
{
    public required string ChoosePaymentMethod { get; set; }
    public required string NoEnabledPayments { get; set; }
    public required string ProceedToPayment { get; set; }
    public required string InvoiceReceiveMessage { get; set; }
    public required string SuccessfullPayment { get; set; }
    public required string ErrorDuringPaymentConfirmation { get; set; }
}