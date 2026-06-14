using AutoVerdikt.WebApi.Endpoints.Admin;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Whitelist;

public sealed class SessionInvalidationTests(AutoVerdiktWebApiFactory factory) : WhitelistTestBase(factory), IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task RemoveFromWhitelist_NextMeCheck_ReturnsNotWhitelisted()
    {
        Factory.WhitelistEnabled = true;
        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        await SeedWhitelistAsync(userId, email);

        using var client = CreateAuthenticatedClient(userId, email);
        await RegisterUserAsync(client, "Alice", email);
        (await IsUserWhitelistedOnMeAsync(client)).ShouldBeTrue();

        using var admin = CreateAdminClient(CreateTestUserId());
        var deleteResponse = await admin.DeleteAsync($"{WhitelistAdminEndpointConstants.WhitelistRoute}/{userId}");
        deleteResponse.EnsureSuccessStatusCode();

        (await IsUserWhitelistedOnMeAsync(client)).ShouldBeFalse();
    }
}
