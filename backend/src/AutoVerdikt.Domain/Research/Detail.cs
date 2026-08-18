namespace AutoVerdikt.Domain.Research;

public record Detail
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }

    public static Detail Create(string name, string value, TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new()
        {
            Id = Guid.CreateVersion7(now),
            Name = name,
            Value = value
        };
    }
}
