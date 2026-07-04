namespace AutoVerdikt.Domain.Research;

public record Detail
{
    public required string Name { get; init; }
    public required string Value { get; init; }
}
