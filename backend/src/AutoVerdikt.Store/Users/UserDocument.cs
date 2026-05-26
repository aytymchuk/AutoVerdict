using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Users;

internal sealed class UserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; }

    [BsonElement("authId")]
    public string AuthId { get; init; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; init; } = string.Empty;

    [BsonElement("registeredAt")]
    public DateTime RegisteredAt { get; init; }
}
