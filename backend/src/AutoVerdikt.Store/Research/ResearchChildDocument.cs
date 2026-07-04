using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

[BsonKnownTypes(
    typeof(NoteDocument),
    typeof(DetailDocument),
    typeof(AttachedFileDocument),
    typeof(QuestionDocument),
    typeof(BadgeDocument))]
internal abstract class ResearchChildDocument
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; init; }

    [BsonElement("researchId")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ResearchId { get; init; }

    [BsonElement("authId")]
    public string AuthId { get; init; } = string.Empty;
}
