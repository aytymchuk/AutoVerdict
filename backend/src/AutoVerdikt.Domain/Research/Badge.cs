namespace AutoVerdikt.Domain.Research;

public record Badge
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required BadgeStatus Status { get; init; }
    public required string Description { get; init; }
    public string? Source { get; init; }

    public static Badge Create(
        string name,
        BadgeStatus status,
        string description,
        TimeProvider timeProvider,
        string? source = null)
    {
        var now = timeProvider.GetUtcNow();
        return new()
        {
            Id = Guid.CreateVersion7(now),
            Name = name,
            Status = status,
            Description = description,
            Source = source
        };
    }
}
