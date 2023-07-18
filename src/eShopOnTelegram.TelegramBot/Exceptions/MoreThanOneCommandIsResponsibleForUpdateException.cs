namespace eShopOnTelegram.TelegramBot.Exceptions;

public class MoreThanOneCommandIsResponsibleForUpdateException : Exception
{
    public MoreThanOneCommandIsResponsibleForUpdateException()
    {

    }

    public MoreThanOneCommandIsResponsibleForUpdateException(string message) : base(message)
    {

    }
}
