using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

internal sealed class AttachedFileDocument : ResearchChildDocument
{
    [BsonElement("kind")]
    public string Kind { get; init; } = string.Empty;

    [BsonElement("fileName")]
    public string FileName { get; init; } = string.Empty;

    [BsonElement("url")]
    public string Url { get; init; } = string.Empty;

    [BsonElement("thumbnailUrl")]
    [BsonIgnoreIfNull]
    public string? ThumbnailUrl { get; init; }

    [BsonElement("agentDescription")]
    [BsonIgnoreIfNull]
    public string? AgentDescription { get; init; }

    [BsonElement("processingStatus")]
    public string ProcessingStatus { get; init; } = string.Empty;

    [BsonElement("rejectionReason")]
    [BsonIgnoreIfNull]
    public string? RejectionReason { get; init; }
}
