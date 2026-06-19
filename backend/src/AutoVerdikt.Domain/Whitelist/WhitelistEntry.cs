namespace AutoVerdikt.Domain.Whitelist;

public record WhitelistEntry
{
    public required Guid Id { get; init; }
    public required string AuthId { get; init; }
    public required string Email { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public Guid? AddedBy { get; init; }

    public static WhitelistEntry Create(
        string authId,
        string email,
        Guid? addedBy,
        TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new()
        {
            Id = Guid.CreateVersion7(now),
            AuthId = authId,
            Email = email,
            CreatedAt = now,
            AddedBy = addedBy
        };
    }
}
