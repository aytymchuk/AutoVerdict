namespace AutoVerdikt.WebApi.Endpoints.Research;

public sealed record CreateResearchDto(string InputMethod, CreateCarDataDto? Car, string? Text);
