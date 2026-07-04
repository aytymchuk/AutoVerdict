using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

internal sealed class BadgeDocument : ResearchChildDocument
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
