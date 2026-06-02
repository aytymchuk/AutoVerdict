using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoVerdikt.WebApi.Endpoints.Users;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Users;

public sealed class UserRegistrationTests(AutoVerdiktWebApiFactory factory) : UsersTestBase(factory), IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task RegisterUser_ValidRequest_ReturnsCreatedWithUserData()
    {
        // Arrange
        var userId = CreateTestUserId();
        var dto = CreateRegistrationDto();
        using var client = CreateAuthenticatedClient(userId);

        // Act
        var response = await client.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<UserAccountDto>();
        body.ShouldNotBeNull();
        body.Name.ShouldBe(dto.Name);
        body.Email.ShouldBe(dto.Email);
        body.Id.ShouldNotBe(Guid.Empty);
        body.RegisteredAt.ShouldNotBe(default);
    }

    [Fact]
    public async Task RegisterUser_DuplicateEmail_ReturnsConflict()
    {
        // Arrange
        var userId = CreateTestUserId();
        var dto = CreateRegistrationDto();
        using var client = CreateAuthenticatedClient(userId);

        var first = await client.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, dto);
        first.StatusCode.ShouldBe(HttpStatusCode.Created);

        // Act — different user, same email → conflict comes from the email uniqueness index
        using var client2 = CreateAuthenticatedClient(CreateTestUserId());
        var second = await client2.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, dto);

        // Assert
        second.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task RegisterUser_DuplicateEmail_ReturnsProblemDetailsWithErrorCode()
    {
        // Arrange
        var userId = CreateTestUserId();
        var dto = CreateRegistrationDto();
        using var client = CreateAuthenticatedClient(userId);

        var first = await client.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, dto);
        first.StatusCode.ShouldBe(HttpStatusCode.Created);

        // Act — different user, same email → conflict from email uniqueness index
        using var client2 = CreateAuthenticatedClient(CreateTestUserId());
        var second = await client2.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, dto);

        // Assert — RFC 9457 Problem Details
        second.Content.Headers.ContentType?.MediaType.ShouldBe("application/problem+json");

        var body = await second.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;

        root.GetProperty("status").GetInt32().ShouldBe(409);
        root.GetProperty("errorCode").GetString().ShouldBe("USR-001");
        root.GetProperty("title").GetString().ShouldBe("User is already registered.");
        root.TryGetProperty("type", out var typeProp).ShouldBeTrue();
        typeProp.GetString()!.ShouldContain("usr-001");
    }

    [Fact]
    public async Task RegisterUser_AlreadyRegisteredSameAuthId_ReturnsProblemDetails()
    {
        // Arrange — same auth identity registers twice
        var userId = CreateTestUserId();
        using var client = CreateAuthenticatedClient(userId);

        var first = await client.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, CreateRegistrationDto());
        first.StatusCode.ShouldBe(HttpStatusCode.Created);

        // Act — same auth id, different dto
        var second = await client.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, CreateRegistrationDto());

        // Assert
        second.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        second.Content.Headers.ContentType?.MediaType.ShouldBe("application/problem+json");
    }

    [Fact]
    public async Task RegisterUser_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var dto = CreateRegistrationDto();
        using var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(UserEndpointConstants.RegisterRoute, dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
