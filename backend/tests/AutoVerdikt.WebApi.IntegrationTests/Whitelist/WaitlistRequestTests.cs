using System.Net;
using System.Net.Http.Json;
using AutoVerdikt.WebApi.Endpoints.Whitelist;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Whitelist;

public sealed class WaitlistRequestTests(AutoVerdiktWebApiFactory factory) : WhitelistTestBase(factory), IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task SubmitWaitlistRequest_ValidRequest_ReturnsCreated()
    {
        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        using var client = CreateAuthenticatedClient(userId, email);
        await RegisterUserAsync(client, "Alice", email);

        var response = await client.PostAsJsonAsync(
            WaitlistEndpointConstants.SubmitRoute,
            new { about = "Looking for my first car" });

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task SubmitWaitlistRequest_DuplicateAuthId_ReturnsConflict()
    {
        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        using var client = CreateAuthenticatedClient(userId, email);
        await RegisterUserAsync(client, "Alice", email);

        await client.PostAsJsonAsync(WaitlistEndpointConstants.SubmitRoute, new { about = "first" });
        var response = await client.PostAsJsonAsync(WaitlistEndpointConstants.SubmitRoute, new { about = "second" });

        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task SubmitWaitlistRequest_DuplicateEmailDifferentAuthId_ReturnsConflict()
    {
        var email = $"{Guid.NewGuid():N}@example.com";

        // First user registers and submits a request
        var userId1 = CreateTestUserId();
        using (var client1 = CreateAuthenticatedClient(userId1, email))
        {
            await RegisterUserAsync(client1, "Alice", email);
            await client1.PostAsJsonAsync(WaitlistEndpointConstants.SubmitRoute, new { about = "first" });
        }

        // Second user is not registered — the handler falls back to currentUser.Email (X-Test-Email header)
        var userId2 = CreateTestUserId();
        using var client2 = CreateAuthenticatedClient(userId2, email);
        var response = await client2.PostAsJsonAsync(WaitlistEndpointConstants.SubmitRoute, new { about = "second" });
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task SubmitWaitlistRequest_Unauthenticated_ReturnsUnauthorized()
    {
        using var client = CreateClient();
        var response = await client.PostAsJsonAsync(WaitlistEndpointConstants.SubmitRoute, new { about = "x" });
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SubmitWaitlistRequest_LongAbout_ReturnsCreated()
    {
        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        using var client = CreateAuthenticatedClient(userId, email);
        await RegisterUserAsync(client, "Alice", email);

        var response = await client.PostAsJsonAsync(
            WaitlistEndpointConstants.SubmitRoute,
            new { about = new string('x', 10_000) });

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}
