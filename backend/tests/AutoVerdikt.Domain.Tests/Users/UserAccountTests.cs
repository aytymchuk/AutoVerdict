using AutoVerdikt.Domain.Users;
using Shouldly;

namespace AutoVerdikt.Domain.Tests.Users;

public class UserAccountTests
{
    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    [Fact]
    public void Create_SetsAllPropertiesFromInputs()
    {
        var fixedTime = new DateTimeOffset(2026, 1, 15, 10, 30, 0, TimeSpan.Zero);

        var user = UserAccount.Create("auth_abc", "Alice", "alice@example.com", new FixedTimeProvider(fixedTime));

        user.AuthId.ShouldBe("auth_abc");
        user.Name.ShouldBe("Alice");
        user.Email.ShouldBe("alice@example.com");
        user.RegisteredAt.ShouldBe(fixedTime);
    }

    [Fact]
    public void Create_AssignsUuidV7Id()
    {
        var user = UserAccount.Create("auth_abc", "Alice", "alice@example.com", TimeProvider.System);

        // UUID v7: the version nibble occupies position 14 in the canonical 8-4-4-4-12 string
        user.Id.ToString()[14].ShouldBe('7');
    }

    [Fact]
    public void Create_IdReflectsRegistrationTimestamp()
    {
        var fixedTime = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);
        var earlier = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var user = UserAccount.Create("auth_abc", "Alice", "alice@example.com", new FixedTimeProvider(fixedTime));
        var olderUser = UserAccount.Create("auth_xyz", "Bob", "bob@example.com", new FixedTimeProvider(earlier));

        // UUID v7 is time-sortable: a later timestamp produces a larger GUID
        user.Id.ShouldBeGreaterThan(olderUser.Id);
    }

    [Fact]
    public void Create_GeneratesUniqueIds()
    {
        var user1 = UserAccount.Create("auth_1", "Alice", "alice@example.com", TimeProvider.System);
        var user2 = UserAccount.Create("auth_2", "Bob", "bob@example.com", TimeProvider.System);

        user1.Id.ShouldNotBe(user2.Id);
    }

    [Fact]
    public void Create_IdNotEmpty()
    {
        var user = UserAccount.Create("auth_abc", "Alice", "alice@example.com", TimeProvider.System);

        user.Id.ShouldNotBe(Guid.Empty);
    }
}
