using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

internal sealed class NoteDocument : ResearchChildDocument
{
    [BsonElement("date")]
    public DateTime Date { get; init; }

    [BsonElement("text")]
    public string Text { get; init; } = string.Empty;
}
