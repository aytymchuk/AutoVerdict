using System.Net;
using System.Net.Http.Json;
using AutoVerdikt.WebApi.Endpoints.Research;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Research.Text;

public sealed class TextResearchTests(AutoVerdiktWebApiFactory factory)
    : ResearchTestBase(factory),
      IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task Create_ReturnsUnprocessableEntity()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());

        var response = await client.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateTextResearch());

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
}
