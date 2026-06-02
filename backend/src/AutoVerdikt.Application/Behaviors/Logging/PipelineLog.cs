using Microsoft.Extensions.Logging;

namespace AutoVerdikt.Application.Behaviors.Logging;

internal static partial class PipelineLog
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Information,
        Message = "Handling {MessageType}: {MessageJson}")]
    internal static partial void HandlingMessage(ILogger logger, string messageType, string messageJson);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Information,
        Message = "Handled {MessageType} in {ElapsedMs}ms")]
    internal static partial void HandledMessage(ILogger logger, string messageType, long elapsedMs);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Warning,
        Message = "Handler for {MessageType} returned failure after {ElapsedMs}ms: {Errors}")]
    internal static partial void HandlerReturnedFailure(ILogger logger, string messageType, long elapsedMs, string errors);

    [LoggerMessage(EventId = 1004, Level = LogLevel.Error,
        Message = "Unhandled exception in handler for {MessageType} after {ElapsedMs}ms")]
    internal static partial void HandlerException(ILogger logger, string messageType, long elapsedMs, Exception exception);
}
