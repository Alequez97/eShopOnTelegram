﻿using eShopOnTelegram.Domain.Dto.Orders;
using eShopOnTelegram.Domain.Requests.Customers;
using eShopOnTelegram.Domain.Responses;
using eShopOnTelegram.Domain.Services.Interfaces;
using eShopOnTelegram.TelegramBot.UnitTests.Extensions;
using eShopOnTelegram.TelegramBot.Worker.Commands;

using FluentAssertions;

using Moq;
using Moq.AutoMock;

using Telegram.Bot;

namespace eShopOnTelegram.TelegramBot.UnitTests.Commands;

public class StartCommandTests
{
    private AutoMocker _autoMocker;
    private Update _update;

    [SetUp]
    public void Setup()
    {
        _autoMocker = new AutoMocker();
        _update = TestData.GetMockedTelegramUpdate();
    }

    [Test]
    public async Task WhenCreateCustomerSuccess_And_NoUnpaidOrderFound_SendTextMessageShouldBeCalledOnce()
    {
        // Arrange
        _autoMocker.GetMock<ICustomerService>()
            .Setup(customerService => customerService.CreateIfNotPresentAsync(It.IsAny<CreateCustomerRequest>()))
            .ReturnsAsync(new ActionResponse() { Status = ResponseStatus.Success });

        _autoMocker.GetMock<IOrderService>()
            .Setup(orderService => orderService.GetUnpaidOrderByTelegramIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<OrderDto>() { Status = ResponseStatus.NotFound });

        var startCommand = _autoMocker.CreateInstance<StartCommand>();

        // Act
        await startCommand.SendResponseAsync(_update);

        // Assert
        _autoMocker.GetMock<ITelegramBotClient>().Invocations.ToList().Should().HaveCount(1);
    }
}
