using System.Diagnostics;
using System.Text.Json;
using AutoVerdikt.Application.Behaviors.Logging;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;

namespace AutoVerdikt.Application.Behaviors;

public sealed class LoggingPipelineBehavior<TMessage, TResponse>(
    ILogger<LoggingPipelineBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : notnull, IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var messageType = typeof(TMessage).Name;
        var messageJson = JsonSerializer.Serialize(message);

        PipelineLog.HandlingMessage(logger, messageType, messageJson);

        var sw = Stopwatch.StartNew();
        try
        {
            var response = await next(message, cancellationToken);
            sw.Stop();

            if (response is IResultBase { IsFailed: true } failedResult)
            {
                var errors = string.Join("; ", failedResult.Errors.Select(e => e.Message));
                PipelineLog.HandlerReturnedFailure(logger, messageType, sw.ElapsedMilliseconds, errors);
            }
            else
            {
                PipelineLog.HandledMessage(logger, messageType, sw.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            PipelineLog.HandlerException(logger, messageType, sw.ElapsedMilliseconds, ex);
            throw;
        }
    }
}
