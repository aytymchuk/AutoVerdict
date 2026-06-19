namespace AutoVerdikt.WebApi.Endpoints.Research;

public sealed record ResearchListItemDto(
    Guid Id,
    string? Name,
    string? Status,
    string? RiskLevel,
    ResearchCarSummaryDto? Car,
    int CreditsSpent,
    DateTimeOffset UpdatedAt);
