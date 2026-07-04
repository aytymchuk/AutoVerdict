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

    public static AttachedFile Create(
        FileKind kind,
        string fileName,
        string url,
        TimeProvider timeProvider,
        string? thumbnailUrl = null,
        string? agentDescription = null,
        ProcessingStatus processingStatus = ProcessingStatus.Pending,
        string? rejectionReason = null)
    {
        var now = timeProvider.GetUtcNow();
        return new()
        {
            Id = Guid.CreateVersion7(now),
            Kind = kind,
            FileName = fileName,
            Url = url,
            ThumbnailUrl = thumbnailUrl,
            AgentDescription = agentDescription,
            ProcessingStatus = processingStatus,
            RejectionReason = rejectionReason
        };
    }
}
