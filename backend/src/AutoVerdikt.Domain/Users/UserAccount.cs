namespace AutoVerdikt.Domain.Users;

public record UserAccount
{
    public Guid Id { get; init; }
    public string AuthId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTimeOffset RegisteredAt { get; init; }

    private UserAccount() { }

    public static UserAccount Create(string authId, string name, string email, TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new() { Id = Guid.CreateVersion7(now), AuthId = authId, Name = name, Email = email, RegisteredAt = now };
    }

    public static UserAccount Reconstitute(Guid id, string authId, string name, string email, DateTimeOffset registeredAt)
        => new() { Id = id, AuthId = authId, Name = name, Email = email, RegisteredAt = registeredAt };
}
