﻿using eShopOnTelegram.TelegramBot.Interfaces;

namespace eShopOnTelegram.TelegramBot.Commands.Groups;

public class MyChatMemberCommand : ITelegramCommand
{
    public Task SendResponseAsync(Update update)
    {
        return Task.CompletedTask;
    }

    public bool IsResponsibleForUpdate(Update update)
    {
        return update.Type == UpdateType.MyChatMember;
    }
}