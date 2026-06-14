using System.Net;
using System.Net.Http.Json;
using AutoVerdikt.Application.Whitelist.Admin.GetWaitlistRequests;
using AutoVerdikt.Domain.Whitelist;
using AutoVerdikt.WebApi.Endpoints.Admin;
using AutoVerdikt.WebApi.Endpoints.Whitelist;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Whitelist;

public sealed class WhitelistAdminTests(AutoVerdiktWebApiFactory factory) : WhitelistTestBase(factory), IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task GetWhitelist_NonAdmin_ReturnsForbidden()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());
        var response = await client.GetAsync($"{WhitelistAdminEndpointConstants.WhitelistRoute}?page=1&pageSize=10");
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetWhitelist_Admin_ReturnsOk()
    {
        using var client = CreateAdminClient(CreateTestUserId());
        var response = await client.GetAsync($"{WhitelistAdminEndpointConstants.WhitelistRoute}?page=1&pageSize=10");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostWhitelist_Admin_CreatesEntry()
    {
        var authId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        using var client = CreateAdminClient(CreateTestUserId());

        var response = await client.PostAsJsonAsync(
            WhitelistAdminEndpointConstants.WhitelistRoute,
            new { authId, email });

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var userClient = CreateAuthenticatedClient(authId, email);
        await RegisterUserAsync(userClient, "Test", email);
        (await IsUserWhitelistedOnMeAsync(userClient)).ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteWhitelist_NonExistent_ReturnsNotFound()
    {
        using var client = CreateAdminClient(CreateTestUserId());
        var response = await client.DeleteAsync($"{WhitelistAdminEndpointConstants.WhitelistRoute}/missing-user");
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ApproveWaitlistRequest_SendsApprovalEmailAndWhitelistsUser()
    {
        Factory.SendGridSpy.Clear();

        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";
        using (var userClient = CreateAuthenticatedClient(userId, email, locale: "pl"))
        {
            await RegisterUserAsync(userClient, "Alice", email);
            await userClient.PostAsJsonAsync(WaitlistEndpointConstants.SubmitRoute, new { about = "tester" });
        }

        using var admin = CreateAdminClient(CreateTestUserId());
        var requests = await admin.GetFromJsonAsync<List<WaitlistRequestListItem>>(
            $"{WhitelistAdminEndpointConstants.WaitlistRequestsRoute}?status=pending");
        requests.ShouldNotBeNull();
        var request = requests.Single(r => r.AuthId == userId);

        var approveResponse = await admin.PostAsync(
            $"{WhitelistAdminEndpointConstants.WaitlistRequestsRoute}/{request.Id}/approve",
            null);
        approveResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        Factory.SendGridSpy.Calls.ShouldContain(c => c.To == email && c.Locale == "pl");

        using var userClientAfter = CreateAuthenticatedClient(userId, email);
        (await IsUserWhitelistedOnMeAsync(userClientAfter)).ShouldBeTrue();
    }

    [Fact]
    public async Task RejectWaitlistRequest_DoesNotSendEmail()
    {
        Factory.SendGridSpy.Clear();
        var userId = CreateTestUserId();
        var email = $"{Guid.NewGuid():N}@example.com";

        using (var userClient = CreateAuthenticatedClient(userId, email))
        {
            await RegisterUserAsync(userClient, "Alice", email);
            await userClient.PostAsJsonAsync(WaitlistEndpointConstants.SubmitRoute, new { about = "tester" });
        }

        using var admin = CreateAdminClient(CreateTestUserId());
        var requests = await admin.GetFromJsonAsync<List<WaitlistRequestListItem>>(
            $"{WhitelistAdminEndpointConstants.WaitlistRequestsRoute}?status=pending");
        var request = requests!.Single(r => r.AuthId == userId);

        var response = await admin.PostAsync(
            $"{WhitelistAdminEndpointConstants.WaitlistRequestsRoute}/{request.Id}/reject",
            null);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        Factory.SendGridSpy.Calls.ShouldBeEmpty();
    }
}
