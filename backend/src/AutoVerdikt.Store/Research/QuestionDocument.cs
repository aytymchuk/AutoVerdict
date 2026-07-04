using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

internal sealed class QuestionDocument : ResearchChildDocument
{
    [BsonElement("questionText")]
    public string QuestionText { get; init; } = string.Empty;

    [BsonElement("answer")]
    [BsonIgnoreIfNull]
    public string? Answer { get; init; }

    [BsonElement("reviewedByAssistant")]
    public bool ReviewedByAssistant { get; init; }
}
