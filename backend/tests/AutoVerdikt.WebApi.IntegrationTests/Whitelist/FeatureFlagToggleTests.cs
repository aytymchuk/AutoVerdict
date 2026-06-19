using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Whitelist;

// Tests where whitelist flag is enabled (default factory)
public sealed class FeatureFlagEnabledTests(AutoVerdiktWebApiFactory factory) : WhitelistTestBase(factory), IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task GetMe_FlagEnabled_NotWhitelisted_ReturnsFalse()
    {
        var userId = CreateTestUserId();
        using var client = CreateAuthenticatedClient(userId);
        await RegisterUserAsync(client, "Alice", $"{Guid.NewGuid():N}@example.com");

        (await IsUserWhitelistedOnMeAsync(client)).ShouldBeFalse();
    }
}

// Tests where whitelist flag is disabled (separate factory instance)
public sealed class FeatureFlagDisabledTests(WhitelistDisabledFactory factory) : WhitelistTestBase(factory), IClassFixture<WhitelistDisabledFactory>
{
    [Fact]
    public async Task GetMe_FlagDisabled_NotWhitelisted_ReturnsTrue()
    {
        var userId = CreateTestUserId();
        using var client = CreateAuthenticatedClient(userId);
        await RegisterUserAsync(client, "Alice", $"{Guid.NewGuid():N}@example.com");

        (await IsUserWhitelistedOnMeAsync(client)).ShouldBeTrue();
    }

    [Fact]
    public async Task GetMe_FlagDisabled_ReturnsIsWhitelistedTrue()
    {
        var userId = CreateTestUserId();
        using var client = CreateAuthenticatedClient(userId);
        await RegisterUserAsync(client, "Alice", $"{Guid.NewGuid():N}@example.com");

        var isWhitelisted = await IsUserWhitelistedOnMeAsync(client);
        isWhitelisted.ShouldBeTrue();
    }
}
