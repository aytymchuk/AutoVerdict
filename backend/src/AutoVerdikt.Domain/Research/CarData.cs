namespace AutoVerdikt.Domain.Research;

public record CarData
{
    public string? Make { get; init; }
    public string? Model { get; init; }
    public int? Year { get; init; }
    public int? MileageKm { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public string? Vin { get; init; }
    public string? FuelType { get; init; }
    public string? Transmission { get; init; }
    public string? EngineDisplacement { get; init; }
    public string? Color { get; init; }
    public string? Condition { get; init; }

    public bool HasMinimumRequiredFields() =>
        !string.IsNullOrWhiteSpace(Make)
        && !string.IsNullOrWhiteSpace(Model)
        && Year is not null
        && MileageKm is not null
        && Price is not null;

    public bool HasAutoNameFields() =>
        !string.IsNullOrWhiteSpace(Make)
        && !string.IsNullOrWhiteSpace(Model)
        && Year is not null;
}
