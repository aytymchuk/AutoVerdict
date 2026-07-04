namespace AutoVerdikt.Domain.Research;

public record Note
{
    public required DateTimeOffset Date { get; init; }
    public required string Text { get; init; }
}
