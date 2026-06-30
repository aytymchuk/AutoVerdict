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
        new("form", car, null);

    protected static CreateResearchDto CreateTextResearch(string? text = null) =>
        new("text", null, text ?? "Volkswagen Golf 2018, 87 200 km, 42 900 PLN. Well maintained.");
}
