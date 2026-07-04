namespace AutoVerdikt.Domain.Research;

public record AttachedFile
{
    public required Guid Id { get; init; }
    public required FileKind Kind { get; init; }
    public required string FileName { get; init; }
    public required string Url { get; init; }
    public string? ThumbnailUrl { get; init; }
    public string? AgentDescription { get; init; }
    public ProcessingStatus ProcessingStatus { get; init; } = ProcessingStatus.Pending;
    public string? RejectionReason { get; init; }
}
