namespace AutoVerdikt.Domain.Users;

public record UserAccount
{
    public Guid Id { get; init; }
    public string ClerkId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTimeOffset RegisteredAt { get; init; }

    private UserAccount() { }

    public static UserAccount Create(string clerkId, string name, string email, TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new() { Id = Guid.CreateVersion7(now), ClerkId = clerkId, Name = name, Email = email, RegisteredAt = now };
    }
}
