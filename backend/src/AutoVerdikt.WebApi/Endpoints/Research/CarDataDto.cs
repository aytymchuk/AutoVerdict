namespace AutoVerdikt.WebApi.Endpoints.Research;

public sealed record CarDataDto(
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
