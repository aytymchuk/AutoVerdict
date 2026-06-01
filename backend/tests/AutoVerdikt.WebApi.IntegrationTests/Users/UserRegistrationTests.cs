using System.Net;
using System.Net.Http.Json;
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
