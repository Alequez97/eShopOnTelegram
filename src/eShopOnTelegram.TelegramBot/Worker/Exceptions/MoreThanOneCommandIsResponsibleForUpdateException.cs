namespace eShopOnTelegram.TelegramBot.Worker.Exceptions;

public class MoreThanOneCommandIsResponsibleForUpdateException : Exception
{
    public MoreThanOneCommandIsResponsibleForUpdateException()
    {

    }

    public MoreThanOneCommandIsResponsibleForUpdateException(string message) : base(message)
    {

    }
}
