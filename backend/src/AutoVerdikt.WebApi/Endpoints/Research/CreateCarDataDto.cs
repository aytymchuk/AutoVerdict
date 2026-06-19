using System.Text.Json.Serialization;

namespace AutoVerdikt.WebApi.Endpoints.Research;

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record CreateCarDataDto(
    string? Make,
    string? Model,
    int? Year,
    int? MileageKm,
    decimal? Price,
    string? Currency,
    string? Vin,
    string? FuelType,
    string? Transmission,
    string? EngineDisplacement,
    string? Color,
    string? Condition);
