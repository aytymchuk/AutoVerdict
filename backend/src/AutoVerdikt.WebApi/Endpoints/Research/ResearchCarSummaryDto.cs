namespace AutoVerdikt.WebApi.Endpoints.Research;

public sealed record ResearchCarSummaryDto(
    string? Make,
    string? Model,
    int? Year,
    int? MileageKm,
    decimal? Price);
