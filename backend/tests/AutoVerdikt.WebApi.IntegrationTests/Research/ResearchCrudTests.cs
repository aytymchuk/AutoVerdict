using System.Net;
using System.Net.Http.Json;
using AutoVerdikt.Application.Common;
using AutoVerdikt.WebApi.Endpoints.Research;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;
using Shouldly;

namespace AutoVerdikt.WebApi.IntegrationTests.Research;

public sealed class ResearchCrudTests(AutoVerdiktWebApiFactory factory)
    : ResearchTestBase(factory),
      IClassFixture<AutoVerdiktWebApiFactory>
{
    [Fact]
    public async Task Get_OtherUsersRecord_ReturnsNotFound()
    {
        using var ownerClient = CreateAuthenticatedClient(CreateTestUserId());
        var createResponse = await ownerClient.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(make: "Volkswagen", model: "Golf", year: 2018)));
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ResearchDetailDto>();

        using var otherClient = CreateAuthenticatedClient(CreateTestUserId());
        var response = await otherClient.GetAsync($"{ResearchEndpointConstants.BaseRoute}/{created!.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Rename_OtherUsersRecord_ReturnsNotFound()
    {
        using var ownerClient = CreateAuthenticatedClient(CreateTestUserId());
        var createResponse = await ownerClient.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(make: "Volkswagen", model: "Golf", year: 2018)));
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ResearchDetailDto>();

        using var otherClient = CreateAuthenticatedClient(CreateTestUserId());
        var response = await otherClient.PatchAsJsonAsync(
            $"{ResearchEndpointConstants.BaseRoute}/{created!.Id}/name",
            new RenameResearchDto("Hijacked"));

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_OtherUsersRecord_ReturnsNotFound()
    {
        using var ownerClient = CreateAuthenticatedClient(CreateTestUserId());
        var createResponse = await ownerClient.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(make: "Volkswagen", model: "Golf", year: 2018)));
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ResearchDetailDto>();

        using var otherClient = CreateAuthenticatedClient(CreateTestUserId());
        var response = await otherClient.DeleteAsync($"{ResearchEndpointConstants.BaseRoute}/{created!.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Rename_ManualName_IsPreserved()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());
        var createResponse = await client.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(
                make: "Volkswagen",
                model: "Golf",
                year: 2018,
                mileageKm: 87200,
                price: 42900)));
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ResearchDetailDto>();

        var renameResponse = await client.PatchAsJsonAsync(
            $"{ResearchEndpointConstants.BaseRoute}/{created!.Id}/name",
            new RenameResearchDto("My Golf"));
        renameResponse.EnsureSuccessStatusCode();
        var renamed = await renameResponse.Content.ReadFromJsonAsync<ResearchDetailDto>();
        renamed!.Name.ShouldBe("My Golf");

        var getResponse = await client.GetAsync($"{ResearchEndpointConstants.BaseRoute}/{created.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<ResearchDetailDto>();
        fetched!.Name.ShouldBe("My Golf");
    }

    [Fact]
    public async Task List_ReturnsOnlyCurrentUsersRecordsSortedByUpdatedAt()
    {
        using var client = CreateAuthenticatedClient(CreateTestUserId());
        using var otherClient = CreateAuthenticatedClient(CreateTestUserId());

        var firstCreate = await client.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(make: "Audi", model: "A3", year: 2017)));
        firstCreate.EnsureSuccessStatusCode();
        var first = await firstCreate.Content.ReadFromJsonAsync<ResearchDetailDto>();

        await Task.Delay(10);

        var secondCreate = await client.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(make: "BMW", model: "320", year: 2019)));
        secondCreate.EnsureSuccessStatusCode();
        var second = await secondCreate.Content.ReadFromJsonAsync<ResearchDetailDto>();

        var otherCreate = await otherClient.PostAsJsonAsync(
            ResearchEndpointConstants.CreateRoute,
            CreateFormResearch(CreateCarData(make: "Toyota", model: "Corolla", year: 2020)));
        otherCreate.EnsureSuccessStatusCode();

        var listResponse = await client.GetAsync(ResearchEndpointConstants.ListRoute);
        listResponse.EnsureSuccessStatusCode();
        var list = await listResponse.Content.ReadFromJsonAsync<PaginatedResult<ResearchListItemDto>>();

        list.ShouldNotBeNull();
        list.Total.ShouldBe(2);
        list.Items.Count.ShouldBe(2);
        list.Items[0].Id.ShouldBe(second!.Id);
        list.Items[1].Id.ShouldBe(first!.Id);
    }
}
