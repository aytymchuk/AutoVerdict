namespace AutoVerdikt.Domain.Whitelist;

public record WaitlistRequest
{
    public required Guid Id { get; init; }
    public required string AuthId { get; init; }
    public required string Email { get; init; }
    public string? About { get; init; }
    public required WaitlistRequestStatus Status { get; init; }
    public required string Locale { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
    public Guid? ReviewedBy { get; init; }
    public DateTimeOffset? ReviewedAt { get; init; }

    public static WaitlistRequest Create(
        string authId,
        string email,
        string? about,
        string locale,
        TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new()
        {
            Id = Guid.CreateVersion7(now),
            AuthId = authId,
            Email = email,
            About = about,
            Status = WaitlistRequestStatus.Pending,
            Locale = locale,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public WaitlistRequest Approve(DateTimeOffset now) =>
        this with { Status = WaitlistRequestStatus.Approved, UpdatedAt = now, ReviewedAt = now };

    public WaitlistRequest Reject(DateTimeOffset now) =>
        this with { Status = WaitlistRequestStatus.Rejected, UpdatedAt = now, ReviewedAt = now };
}
