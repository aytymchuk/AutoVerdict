using Microsoft.Extensions.Logging;

namespace AutoVerdikt.Application.Behaviors.Logging;

internal static partial class PipelineLog
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Information,
        Message = "Handling {MessageType}")]
    internal static partial void HandlingMessage(ILogger logger, string messageType);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Information,
        Message = "Handled {MessageType}")]
    internal static partial void HandledMessage(ILogger logger, string messageType);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Warning,
        Message = "Handler for {MessageType} returned failure: {Errors}")]
    internal static partial void HandlerReturnedFailure(ILogger logger, string messageType, string errors);

    [LoggerMessage(EventId = 1004, Level = LogLevel.Error,
        Message = "Unhandled exception in handler for {MessageType}")]
    internal static partial void HandlerException(ILogger logger, string messageType, Exception exception);
}
