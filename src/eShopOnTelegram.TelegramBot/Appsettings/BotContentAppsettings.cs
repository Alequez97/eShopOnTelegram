namespace eShopOnTelegram.TelegramBot.Appsettings;

public class BotContentAppsettings
{
    public required BotCommonContent Common { get; set; }

    public required BotOrderContent Order { get; set; }

    public required BotPaymentContent Payment { get; set; }
}

public class BotCommonContent
{
    public string? StartError { get; set; }
    public string? WelcomeText { get; set; }
    public string? OpenShopButtonText { get; set; }
    public string? UnknownCommandText { get; set; }
    public string? DefaultErrorMessage { get; set; }
}

public class BotOrderContent
{
    public string? CreateErrorMessage { get; set; }
    public string? InvoiceGenerationFailedErrorMessage { get; set; }
    public string? OrderNumberTitle { get; set; }
}

public class BotPaymentContent
{
    public string? ChoosePaymentMethod { get; set; }
    public string? NoEnabledPayments { get; set; }
    public string? SuccessfullPayment { get; set; }
    public string? ErrorDuringPaymentConfirmation { get; set; }
}