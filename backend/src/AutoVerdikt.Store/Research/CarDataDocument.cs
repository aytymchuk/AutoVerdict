using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoVerdikt.Store.Research;

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
