using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

internal sealed class DetailDocument : ResearchChildDocument
{
    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("value")]
    public string Value { get; init; } = string.Empty;
}
