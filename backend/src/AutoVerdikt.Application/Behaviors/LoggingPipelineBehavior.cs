using System.Diagnostics;
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

        using var activity = MediatorPipelineActivity.Source.StartActivity(messageType, ActivityKind.Internal);
        activity?.SetTag(MediatorPipelineActivity.Tags.MessageType, typeof(TMessage).FullName ?? messageType);

        PipelineLog.HandlingMessage(logger, messageType);

        try
        {
            var response = await next(message, cancellationToken);

            if (response is IResultBase { IsFailed: true } failedResult)
            {
                var errors = string.Join("; ", failedResult.Errors.Select(e => e.Message));
                activity?.SetTag(MediatorPipelineActivity.Tags.Errors, errors);
                activity?.SetStatus(ActivityStatusCode.Error, errors);
                PipelineLog.HandlerReturnedFailure(logger, messageType, errors);
            }
            else
            {
                activity?.SetStatus(ActivityStatusCode.Ok);
                PipelineLog.HandledMessage(logger, messageType);
            }

            return response;
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            PipelineLog.HandlerException(logger, messageType, ex);
            throw;
        }
        finally
        {
            if (activity is not null)
            {
                activity.Stop();
                activity.SetTag(MediatorPipelineActivity.Tags.DurationMs, activity.Duration.TotalMilliseconds);
            }
        }
    }
}
