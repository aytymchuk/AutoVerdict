namespace AutoVerdikt.WebApi.Endpoints.Research;

public sealed record ResearchDetailDto(
    Guid Id,
    string? Name,
    string? Status,
    string? RiskLevel,
    string InputMethod,
    CarDataDto? Car,
    string? Description,
    string? DescriptionSource,
    int CreditsSpent,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
