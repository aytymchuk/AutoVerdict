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
    public async Task Create_ReturnsCreatedWithExtractedCarData()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());
        const string listingText = "Volkswagen Golf 2018, 87 200 km, 42 900 PLN. Well maintained.";

        var response = await client.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateTextResearch(listingText));

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<ResearchDetailDto>();
        body.ShouldNotBeNull();
        body!.InputMethod.ShouldBe("text");
        body.Car.ShouldNotBeNull();
        body.Car!.Make.ShouldBe("Volkswagen");
        body.Car.Model.ShouldBe("Golf");
        body.Car.Year.ShouldBe(2018);
        body.Description.ShouldNotBeNullOrWhiteSpace();
        body.DescriptionSource.ShouldBe("aiGeneratedFromText");
        body.InitialPrompt.ShouldBe(listingText);
    }
}
