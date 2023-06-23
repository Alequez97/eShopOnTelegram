namespace eShopOnTelegram.TelegramBot.Extensions;

public static class ConfigurationExtensions
{
    public static T GetSection<T>(this IConfiguration configuration, string sectionName) where T : class
    {
        return configuration.GetSection(sectionName).Get<T>();
    }
}
