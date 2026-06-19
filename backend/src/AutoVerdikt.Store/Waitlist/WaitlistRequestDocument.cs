using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Waitlist;

internal sealed class WaitlistRequestDocument
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("authId")]
    public string AuthId { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("about")]
    [BsonIgnoreIfNull]
    public string? About { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    [BsonElement("locale")]
    public string Locale { get; set; } = "en";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("reviewedBy")]
    [BsonIgnoreIfNull]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? ReviewedBy { get; set; }

    [BsonElement("reviewedAt")]
    [BsonIgnoreIfNull]
    public DateTime? ReviewedAt { get; set; }
}
