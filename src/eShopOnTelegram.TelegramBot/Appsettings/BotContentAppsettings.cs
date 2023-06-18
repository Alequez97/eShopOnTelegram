namespace eShopOnTelegram.TelegramBot.Appsettings;

public class BotContentAppsettings
{
    public required BotCommonContent Common { get; set; }

    public required BotPaymentContent Payment { get; set; }
}

public class BotCommonContent
{
    public string? WelcomeText { get; set; }
    public string? OpenShopButtonText { get; set; }
    public string? DefaultErrorMessage { get; set; }

}

public class BotPaymentContent
{
    public string? ChoosePaymentMethod { get; set; }
    public string? NoEnabledPayments { get; set; }
}