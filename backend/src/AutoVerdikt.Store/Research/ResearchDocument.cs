using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

internal sealed class ResearchDocument
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; init; }

    [BsonElement("authId")]
    public string AuthId { get; init; } = string.Empty;

    [BsonElement("name")]
    [BsonIgnoreIfNull]
    public string? Name { get; init; }

    [BsonElement("status")]
    [BsonIgnoreIfNull]
    public string? Status { get; init; }

    [BsonElement("riskLevel")]
    [BsonIgnoreIfNull]
    public string? RiskLevel { get; init; }

    [BsonElement("inputMethod")]
    public string InputMethod { get; init; } = string.Empty;

    [BsonElement("car")]
    [BsonIgnoreIfNull]
    public CarDataDocument? Car { get; init; }

    [BsonElement("description")]
    [BsonIgnoreIfNull]
    public string? Description { get; init; }

    [BsonElement("descriptionSource")]
    [BsonIgnoreIfNull]
    public string? DescriptionSource { get; init; }

    [BsonElement("initialPrompt")]
    [BsonIgnoreIfNull]
    public string? InitialPrompt { get; init; }

    [BsonElement("creditsSpent")]
    public int CreditsSpent { get; init; }

    [BsonElement("isNameManual")]
    public bool IsNameManual { get; init; }

    [BsonElement("lastAnalyzedAt")]
    [BsonIgnoreIfNull]
    public DateTime? LastAnalyzedAt { get; init; }

    [BsonElement("retentionPolicy")]
    [BsonIgnoreIfNull]
    public string? RetentionPolicy { get; init; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; init; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; init; }
}
