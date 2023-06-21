namespace eShopOnTelegram.TelegramBot.Appsettings;

public class BotContentAppsettings
{
    public required BotCommonContent Common { get; set; }

    public required BotOrderContent Order { get; set; }

    public required BotPaymentContent Payment { get; set; }
}

public class BotCommonContent
{
    public required string StartError { get; set; }
    public required string WelcomeText { get; set; }
    public required string OpenShopButtonText { get; set; }
    public required string UnknownCommandText { get; set; }
    public required string DefaultErrorMessage { get; set; }
}

public class BotOrderContent
{
    public required string ShowUnpaidOrder { get; set; }
    public required string NoUnpaidOrderFound { get; set; }
    public required string AlreadyPaidOrExpired { get; set; }
    public required string CreateErrorMessage { get; set; }
    public required string InvoiceGenerationFailedErrorMessage { get; set; }
    public required string OrderNumberTitle { get; set; }
}

public class BotPaymentContent
{
    public required string ChoosePaymentMethod { get; set; }
    public required string NoEnabledPayments { get; set; }
    public required string ProceedToPayment { get; set; }
    public required string InvoiceReceiveMessage { get; set; }
    public required string SuccessfullPayment { get; set; }
    public required string ErrorDuringPaymentConfirmation { get; set; }
}