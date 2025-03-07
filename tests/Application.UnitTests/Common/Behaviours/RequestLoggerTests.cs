﻿using GerPros_Backend_API.Application.Common.Behaviours;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace GerPros_Backend_API.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateBrandCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateBrandCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<CreateBrandCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateBrandCommand { Name = "title" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateBrandCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateBrandCommand
        {
            Name = "title" 
        }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
