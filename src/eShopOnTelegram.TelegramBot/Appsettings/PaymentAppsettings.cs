namespace eShopOnTelegram.TelegramBot.Appsettings;

public class PaymentAppsettings
{
    public required Card Card { get; set; }

    public required Plisio Plisio { get; set; }

    public bool AllPaymentsDisabled => !Card.Enabled && !Plisio.Enabled;
}

public class Card
{
    public bool Enabled { get; set; }

    public required string ApiToken { get; set; }

    public required string Currency { get; set; }
}

public class Plisio
{
    public bool Enabled { get; set; }

    public required string ApiToken { get; set; }
}