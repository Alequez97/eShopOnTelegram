namespace eShopOnTelegram.TelegramBot.UnitTests.TestDataDir;

public static class TestData
{
    public static Update GetMockedTelegramUpdate()
    {
        var user = new User()
        {
            Id = -1,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Username = "TestUserName"
        };

        var message = new Message()
        {
            From = user,
            Chat = new Chat()
            {
                Id = -1
            }
        };

        return new Update()
        {
            Message = message
        };
    }
}
