using System.ComponentModel;
using AutoVerdikt.Application.AI.Extraction;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create.StartText;

[Description("Structured facts extracted from a used car listing. All fields optional.")]
[ExtractionSchema("""
    You are a precise data extractor for used car listings.
    Extract vehicle facts from the provided text.
    The input may be in English, Ukrainian, or Polish.
    Return null for any field not explicitly present in the text.
    Do not infer, guess, or hallucinate values.
    Normalise mileage to kilometres (integer) and price to a numeric amount without currency symbols.
    """)]
public sealed class CarListingFacts
{
    [Description("Vehicle manufacturer / make, e.g. Volkswagen, BMW")]
    public string? Make { get; init; }

    [Description("Vehicle model name, e.g. Golf, 320d")]
    public string? Model { get; init; }

    [Description("Model year as a 4-digit integer")]
    public int? Year { get; init; }

    [Description("Odometer reading in kilometres as an integer")]
    public int? MileageKm { get; init; }

    [Description("Asking price as a numeric value without currency symbol")]
    public decimal? Price { get; init; }

    [Description("ISO 4217 3-letter currency code (PLN, EUR, USD); null if not mentioned")]
    public string? Currency { get; init; }

    [Description("17-character Vehicle Identification Number; null if not mentioned")]
    public string? Vin { get; init; }

    [Description("Fuel type, e.g. petrol, diesel, hybrid, electric")]
    public string? FuelType { get; init; }

    [Description("Transmission type, e.g. manual, automatic")]
    public string? Transmission { get; init; }

    [Description("Engine displacement, e.g. 2.0 TDI, 1.6")]
    public string? EngineDisplacement { get; init; }

    [Description("Exterior colour")]
    public string? Color { get; init; }

    [Description("Overall condition as stated in the listing")]
    public string? Condition { get; init; }

    [Description("Short summary of the listing in the same language as the input")]
    public string? Description { get; init; }

    public CarData ToCarData() =>
        new()
        {
            Make = Make,
            Model = Model,
            Year = Year,
            MileageKm = MileageKm,
            Price = Price,
            Currency = Currency,
            Vin = Vin,
            FuelType = FuelType,
            Transmission = Transmission,
            EngineDisplacement = EngineDisplacement,
            Color = Color,
            Condition = Condition
        };
}
