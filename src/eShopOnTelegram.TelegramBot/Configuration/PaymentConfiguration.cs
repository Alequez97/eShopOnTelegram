namespace eShopOnTelegram.TelegramBot.Configuration;

public class PaymentConfiguration
{
    public Card? Card { get; set; }

    public Plisio? Plisio { get; set; }

    public bool AllPaymentsDisabled => !Card.Enabled && !Plisio.Enabled;
}

public class Card
{
    public bool Enabled { get; set; }

    public string? ApiToken { get; set; }
}

public class Plisio
{
    public bool Enabled { get; set; }

    public string? ApiToken { get; set; }
}