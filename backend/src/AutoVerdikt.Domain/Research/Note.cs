namespace AutoVerdikt.Domain.Research;

public record Note
{
    public required Guid Id { get; init; }
    public required DateTimeOffset Date { get; init; }
    public required string Text { get; init; }

    public static Note Create(DateTimeOffset date, string text, TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new()
        {
            Id = Guid.CreateVersion7(now),
            Date = date,
            Text = text
        };
    }
}
