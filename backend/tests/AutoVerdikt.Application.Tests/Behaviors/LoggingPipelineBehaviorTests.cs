using AutoVerdikt.Application.Behaviors;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace AutoVerdikt.Application.Tests.Behaviors;

// Minimal test message types — IRequest<T> satisfies the IMessage constraint on the behavior
internal sealed record SuccessCommand : IRequest<Result<string>>;
internal sealed record FailureCommand : IRequest<Result<string>>;
internal sealed record ThrowingCommand : IRequest<Result<string>>;

public class LoggingPipelineBehaviorTests
{
    private readonly Mock<ILogger<LoggingPipelineBehavior<SuccessCommand, Result<string>>>> _successLogger = new();
    private readonly Mock<ILogger<LoggingPipelineBehavior<FailureCommand, Result<string>>>> _failureLogger = new();
    private readonly Mock<ILogger<LoggingPipelineBehavior<ThrowingCommand, Result<string>>>> _throwingLogger = new();

    [Fact]
    public async Task Handle_SuccessfulResult_LogsStartAndCompletion()
    {
        var behavior = new LoggingPipelineBehavior<SuccessCommand, Result<string>>(_successLogger.Object);

        var response = await behavior.Handle(
            new SuccessCommand(),
            (_, _) => ValueTask.FromResult(Result.Ok("value")),
            CancellationToken.None);

        response.IsSuccess.ShouldBeTrue();
        VerifyLogLevel(_successLogger, LogLevel.Information, Times.Exactly(2)); // HandlingMessage + HandledMessage
        VerifyLogLevel(_successLogger, LogLevel.Warning, Times.Never());
        VerifyLogLevel(_successLogger, LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task Handle_FailedResult_LogsWarningWithErrors()
    {
        var behavior = new LoggingPipelineBehavior<FailureCommand, Result<string>>(_failureLogger.Object);

        var response = await behavior.Handle(
            new FailureCommand(),
            (_, _) => ValueTask.FromResult(Result.Fail<string>("something went wrong")),
            CancellationToken.None);

        response.IsFailed.ShouldBeTrue();
        VerifyLogLevel(_failureLogger, LogLevel.Warning, Times.Once());
        VerifyLogLevel(_failureLogger, LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task Handle_Exception_LogsErrorAndRethrows()
    {
        var behavior = new LoggingPipelineBehavior<ThrowingCommand, Result<string>>(_throwingLogger.Object);
        var exception = new InvalidOperationException("unexpected failure");

        await Should.ThrowAsync<InvalidOperationException>(async () =>
            await behavior.Handle(
                new ThrowingCommand(),
                (_, _) => throw exception,
                CancellationToken.None));

        VerifyLogLevel(_throwingLogger, LogLevel.Error, Times.Once());
        VerifyLogLevel(_throwingLogger, LogLevel.Warning, Times.Never());
    }

    [Fact]
    public async Task Handle_Exception_LogsExactException()
    {
        var behavior = new LoggingPipelineBehavior<ThrowingCommand, Result<string>>(_throwingLogger.Object);
        var exception = new InvalidOperationException("unexpected failure");

        await Should.ThrowAsync<InvalidOperationException>(async () =>
            await behavior.Handle(
                new ThrowingCommand(),
                (_, _) => throw exception,
                CancellationToken.None));

        _throwingLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private static void VerifyLogLevel<T>(Mock<ILogger<T>> logger, LogLevel level, Times times) =>
        logger.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
}
