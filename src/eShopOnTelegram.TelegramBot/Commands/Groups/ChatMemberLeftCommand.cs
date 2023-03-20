﻿using eShopOnTelegram.TelegramBot.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Groups;

public class ChatMemberLeftCommand : ITelegramCommand
{
    public Task SendResponseAsync(Update update)
    {
        return Task.CompletedTask;
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Message?.Type == MessageType.ChatMemberLeft;
    }
}