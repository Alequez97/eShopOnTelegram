﻿using TelegramBot.Commands.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Groups;

public class ChatMemberAddedCommand : ITelegramCommand
{
    public Task SendResponseAsync(Update update)
    {
        return Task.CompletedTask;
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Message?.Type == MessageType.ChatMembersAdded;
    }
}