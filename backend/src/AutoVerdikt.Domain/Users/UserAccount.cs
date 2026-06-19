namespace AutoVerdikt.Domain.Users;

public record UserAccount
{
    public required Guid Id { get; init; }
    public required string AuthId { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required DateTimeOffset RegisteredAt { get; init; }
    public WhitelistStatus WhitelistStatus { get; init; } = WhitelistStatus.None;

    public static UserAccount Create(string authId, string name, string email, TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new() { Id = Guid.CreateVersion7(now), AuthId = authId, Name = name, Email = email, RegisteredAt = now };
    }

    public UserAccount ChangeWhitelistStatus(WhitelistStatus status) =>
        this with { WhitelistStatus = status };
}
