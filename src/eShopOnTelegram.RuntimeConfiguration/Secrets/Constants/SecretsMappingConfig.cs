using eShopOnTelegram.Utils.Configuration;

namespace eShopOnTelegram.RuntimeConfiguration.Secrets.Constants;

public class SecretsMappingConfig
{
    public List<SecretsMappingPrivateConfigItem> PrivateConfig =>
    new()
    {
            new SecretsMappingPrivateConfigItem
            {
                DisplayName = "Telegram token",
                PublicSecretName = SecretPublicName.TelegramToken,
                PrivateSecretName = $"{nameof(AppSettings)}--{nameof(TelegramBotSettings)}--Token"
            },
            new SecretsMappingPrivateConfigItem
            {
                DisplayName = "Telegram bot owner id",
                PublicSecretName = SecretPublicName.TelegramBotOwnerId,
                PrivateSecretName = $"{nameof(AppSettings)}--{nameof(TelegramBotSettings)}--BotOwnerTelegramId"
            },
            new SecretsMappingPrivateConfigItem
            {
                DisplayName = "Bank card payments api token",
                PublicSecretName = SecretPublicName.Payments_BankCardToken,
                PrivateSecretName = $"{nameof(AppSettings)}--{nameof(PaymentSettings)}--{nameof(Card)}--ApiToken"
            },
            // TODO: Restore when pliso will be fully implemented
            //new SecretsMappingPrivateConfigItem
            //{
            //    DisplayName = "Plisio payments api token",
            //    PublicSecretName = SecretPublicName.Payments_PlisioApiToken,
            //    PrivateSecretName = $"{nameof(AppSettings)}--{nameof(PaymentSettings)}--{nameof(Plisio)}--ApiToken"
            //},
    };

    public List<SecretsMappingConfigItem> PublicConfig =>
        PrivateConfig.Select(privateConfigItem =>
            new SecretsMappingConfigItem()
            {
                DisplayName = privateConfigItem.DisplayName,
                PublicSecretName = privateConfigItem.PublicSecretName
            }).ToList();
}

public class SecretsMappingConfigItem
{
    public required string DisplayName { get; set; }

    public required string PublicSecretName { get; set; }
}

public class SecretsMappingPrivateConfigItem : SecretsMappingConfigItem
{
    public required string PrivateSecretName { get; set; }
}
