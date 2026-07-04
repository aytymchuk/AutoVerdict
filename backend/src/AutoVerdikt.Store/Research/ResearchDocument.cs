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

    [BsonElement("notes")]
    public List<NoteDocument> Notes { get; init; } = [];

    [BsonElement("details")]
    public List<DetailDocument> Details { get; init; } = [];

    [BsonElement("files")]
    public List<AttachedFileDocument> Files { get; init; } = [];

    [BsonElement("questions")]
    public List<QuestionDocument> Questions { get; init; } = [];

    [BsonElement("badges")]
    public List<BadgeDocument> Badges { get; init; } = [];
}

internal sealed class CarDataDocument
{
    [BsonElement("make")]
    [BsonIgnoreIfNull]
    public string? Make { get; init; }

    [BsonElement("model")]
    [BsonIgnoreIfNull]
    public string? Model { get; init; }

    [BsonElement("year")]
    [BsonIgnoreIfNull]
    public int? Year { get; init; }

    [BsonElement("mileageKm")]
    [BsonIgnoreIfNull]
    public int? MileageKm { get; init; }

    [BsonElement("price")]
    [BsonRepresentation(BsonType.Decimal128)]
    [BsonIgnoreIfNull]
    public decimal? Price { get; init; }

    [BsonElement("currency")]
    [BsonIgnoreIfNull]
    public string? Currency { get; init; }

    [BsonElement("vin")]
    [BsonIgnoreIfNull]
    public string? Vin { get; init; }

    [BsonElement("fuelType")]
    [BsonIgnoreIfNull]
    public string? FuelType { get; init; }

    [BsonElement("transmission")]
    [BsonIgnoreIfNull]
    public string? Transmission { get; init; }

    [BsonElement("engineDisplacement")]
    [BsonIgnoreIfNull]
    public string? EngineDisplacement { get; init; }

    [BsonElement("color")]
    [BsonIgnoreIfNull]
    public string? Color { get; init; }

    [BsonElement("condition")]
    [BsonIgnoreIfNull]
    public string? Condition { get; init; }
}

internal sealed class NoteDocument
{
    [BsonElement("date")]
    public DateTime Date { get; init; }

    [BsonElement("text")]
    public string Text { get; init; } = string.Empty;
}

internal sealed class DetailDocument
{
    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("value")]
    public string Value { get; init; } = string.Empty;
}

internal sealed class AttachedFileDocument
{
    [BsonElement("id")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; init; }

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

internal sealed class QuestionDocument
{
    [BsonElement("questionText")]
    public string QuestionText { get; init; } = string.Empty;

    [BsonElement("answer")]
    [BsonIgnoreIfNull]
    public string? Answer { get; init; }

    [BsonElement("reviewedByAssistant")]
    public bool ReviewedByAssistant { get; init; }
}

internal sealed class BadgeDocument
{
    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; init; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; init; } = string.Empty;

    [BsonElement("source")]
    [BsonIgnoreIfNull]
    public string? Source { get; init; }
}
