using AutoVerdikt.WebApi.Endpoints.Research;
using AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

namespace AutoVerdikt.WebApi.IntegrationTests.Research;

public abstract class ResearchTestBase(AutoVerdiktWebApiFactory factory) : BaseFixture(factory)
{
    protected static CreateCarDataDto CreateCarData(
        string? make = null,
        string? model = null,
        int? year = null,
        int? mileageKm = null,
        decimal? price = null) =>
        new(make, model, year, mileageKm, price, null, null, null, null, null, null, null);

    protected static CreateResearchDto CreateFormResearch(CreateCarDataDto car) =>
        new("form", car);

    protected static CreateResearchDto CreateTextResearch() =>
        new("text", null);
}
