using Microsoft.Extensions.Logging;
using Moq;

namespace SkyStorage.Application.Tests.Extensions;

public static class MoqLoggerExtension
{
    public static void VerifyLogger<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            times);
    }
}