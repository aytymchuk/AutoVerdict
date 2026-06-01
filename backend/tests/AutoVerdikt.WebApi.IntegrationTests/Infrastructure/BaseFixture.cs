using AutoFixture;
using AutoVerdikt.WebApi.Endpoints.Users;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public abstract class BaseFixture(AutoVerdiktWebApiFactory factory)
{
    protected AutoVerdiktWebApiFactory Factory { get; } = factory;

    protected Fixture Specimens { get; } = new();

    protected HttpClient CreateClient() => Factory.CreateClient();

    protected HttpClient CreateAuthenticatedClient(string userId)
    {
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeaderName, userId);
        return client;
    }

    protected string CreateTestUserId() => Specimens.Create<Guid>().ToString();

    protected UserRegistrationDto CreateRegistrationDto() =>
        new(Specimens.Create<string>(), $"{Specimens.Create<Guid>():N}@example.com");
}
