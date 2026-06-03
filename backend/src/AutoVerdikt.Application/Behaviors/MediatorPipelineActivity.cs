using System.Diagnostics;

namespace AutoVerdikt.Application.Behaviors;

public static class MediatorPipelineActivity
{
    public const string ActivitySourceName = "AutoVerdict.Mediator";

    public static readonly ActivitySource Source = new(ActivitySourceName);

    public static class Tags
    {
        public const string MessageType = "mediator.message_type";
        public const string Errors = "mediator.errors";
        public const string DurationMs = "mediator.duration_ms";
    }
}
