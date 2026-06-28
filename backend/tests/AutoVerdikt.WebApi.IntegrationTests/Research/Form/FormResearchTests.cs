using System.Net;
using System.Net.Http.Json;
using System.Text;
using AutoVerdikt.WebApi.Endpoints.Research;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Research.Form;

public sealed class FormResearchTests(AutoVerdiktWebApiFactory factory)
    : ResearchTestBase(factory),
      IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task Create_WithMinimumFields_ReturnsPendingStatus()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());

        var response = await client.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(
                make: "Volkswagen",
                model: "Golf",
                year: 2018,
                mileageKm: 87200,
                price: 42900)));

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<ResearchDetailDto>();
        body.ShouldNotBeNull();
        body.Status.ShouldBe("pending");
        body.Name.ShouldBe("Volkswagen Golf 2018");
    }

    [Fact]
    public async Task Create_WithPartialData_SetsStatusDraft()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());

        var response = await client.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(make: "Volkswagen", model: "Golf", year: 2018)));

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<ResearchDetailDto>();
        body.ShouldNotBeNull();
        body.Status.ShouldBe("draft");
    }

    [Fact]
    public async Task Create_UnknownCarField_ReturnsBadRequest()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());
        // Raw JSON is intentional: DTOs cannot express unknown properties for this deserialization test.
        const string payload = """
            {
              "inputMethod": "form",
              "car": {
                "make": "Volkswagen",
                "unknownField": "value"
              }
            }
            """;

        var response = await client.PostAsync(
            ResearchEndpointConstants.CreateRoute,
            new StringContent(payload, Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
