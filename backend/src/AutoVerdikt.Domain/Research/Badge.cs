namespace AutoVerdikt.Domain.Research;

public record Badge
{
    public required string Name { get; init; }
    public required BadgeStatus Status { get; init; }
    public required string Description { get; init; }
    public string? Source { get; init; }
}
