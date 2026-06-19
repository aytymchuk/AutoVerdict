using System.Net;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Whitelist;

public sealed class WhitelistGateTests(AutoVerdiktWebApiFactory factory) : WhitelistTestBase(factory), IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task GetMe_FlagEnabled_UserInWhitelist_ReturnsIsWhitelistedTrue()
    {
        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        await SeedWhitelistAsync(userId, email);

        using var client = CreateAuthenticatedClient(userId, email);
        await RegisterUserAsync(client, "Alice", email);

        var isWhitelisted = await IsUserWhitelistedOnMeAsync(client);
        isWhitelisted.ShouldBeTrue();
    }

    [Fact]
    public async Task GetMe_FlagEnabled_UserNotInWhitelist_ReturnsIsWhitelistedFalse()
    {
        var userId = CreateTestUserId();
        using var client = CreateAuthenticatedClient(userId);
        await RegisterUserAsync(client, "Alice", $"{Guid.NewGuid():N}@example.com");

        var isWhitelisted = await IsUserWhitelistedOnMeAsync(client);
        isWhitelisted.ShouldBeFalse();
    }

    [Fact]
    public async Task ProductAccess_FlagEnabled_WhitelistedUser_ReturnsOk()
    {
        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        await SeedWhitelistAsync(userId, email);

        using var client = CreateAuthenticatedClient(userId, email);
        await RegisterUserAsync(client, "Alice", email);

        var response = await GetProductAccessAsync(client);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ProductAccess_FlagEnabled_NotWhitelisted_ReturnsForbidden()
    {
        var userId = CreateTestUserId();
        using var client = CreateAuthenticatedClient(userId);
        await RegisterUserAsync(client, "Alice", $"{Guid.NewGuid():N}@example.com");

        var response = await GetProductAccessAsync(client);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
