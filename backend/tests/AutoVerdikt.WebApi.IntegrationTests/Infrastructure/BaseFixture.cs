using System.Net.Http.Json;
using AutoFixture;
using AutoVerdikt.WebApi.Endpoints.Admin;
using AutoVerdikt.WebApi.Endpoints.Product;
using AutoVerdikt.WebApi.Endpoints.Users;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public abstract class BaseFixture(AutoVerdiktWebApiFactory factory)
{
    protected AutoVerdiktWebApiFactory Factory { get; } = factory;

    protected Fixture Specimens { get; } = new();

    protected HttpClient CreateClient() => Factory.CreateClient();

    protected HttpClient CreateAuthenticatedClient(string userId, string? email = null, string locale = "en")
    {
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeaderName, userId);
        if (email is not null)
            client.DefaultRequestHeaders.Add(TestAuthHandler.EmailHeaderName, email);
        client.DefaultRequestHeaders.Add(TestAuthHandler.LocaleHeaderName, locale);
        return client;
    }

    protected HttpClient CreateAdminClient(string userId, string? email = null)
    {
        var client = CreateAuthenticatedClient(userId, email);
        client.DefaultRequestHeaders.Add(TestAuthHandler.RoleHeaderName, "admin");
        return client;
    }

    protected async Task SeedWhitelistAsync(string authId, string email)
    {
        using var admin = CreateAdminClient(CreateTestUserId());
        var response = await admin.PostAsJsonAsync(
            WhitelistAdminEndpointConstants.WhitelistRoute,
            new AddWhitelistEntryDto(authId, email));
        response.EnsureSuccessStatusCode();
    }

    protected async Task RegisterUserAsync(HttpClient client, string name, string email)
    {
        var response = await client.PostAsJsonAsync(
            UserEndpointConstants.RegisterRoute,
            new UserRegistrationDto(name, email));
        response.EnsureSuccessStatusCode();
    }

    protected string CreateTestUserId() => $"auth0|{Specimens.Create<Guid>()}";

    protected UserRegistrationDto CreateRegistrationDto() =>
        new(Specimens.Create<string>(), $"{Specimens.Create<Guid>():N}@example.com");

    protected async Task<bool> IsUserWhitelistedOnMeAsync(HttpClient client)
    {
        var response = await client.GetAsync(UserEndpointConstants.GetCurrentRoute);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<UserMeResponse>();
        return body?.IsWhitelisted ?? false;
    }

    protected sealed record UserMeResponse(
        Guid Id,
        string Name,
        string Email,
        DateTimeOffset RegisteredAt,
        bool IsWhitelisted);

    protected Task<HttpResponseMessage> GetProductAccessAsync(HttpClient client) =>
        client.GetAsync(ProductEndpointConstants.AccessRoute);
}
