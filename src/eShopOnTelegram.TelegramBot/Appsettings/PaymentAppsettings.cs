namespace eShopOnTelegram.TelegramBot.Appsettings;

public class PaymentAppsettings
{
    public required string MainCurrency { get; set; }

    public required Card Card { get; set; }

    public required Plisio Plisio { get; set; }

    public bool AllPaymentsDisabled => !Card.Enabled && !Plisio.Enabled;
}

public class Card
{
    public bool Enabled => !string.IsNullOrWhiteSpace(ApiToken);

    public required string ApiToken { get; set; }
}

public class Plisio
{
    public bool Enabled => !string.IsNullOrWhiteSpace(ApiToken);

    public required string ApiToken { get; set; }

    public required string CryptoCurrency { get; set; }
}