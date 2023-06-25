namespace eShopOnTelegram.TelegramBot.Exceptions;

public class GroupIsNotCreatedException : Exception
{
    public GroupIsNotCreatedException() : base("Telegram group to receive notifications is not created")
    {
        
    }
}
