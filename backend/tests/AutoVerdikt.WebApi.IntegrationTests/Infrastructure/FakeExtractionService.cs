using AutoVerdikt.Application.AI.Extraction;
using AutoVerdikt.Application.AI.Extraction.Errors;
using AutoVerdikt.Application.Research.Create.StartText;
using FluentResults;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public sealed class FakeExtractionService : IExtractionService
{
    public bool ShouldFail { get; set; }

    public Task<Result<ExtractionOutcome<T>>> ExtractAsync<T>(string input, CancellationToken ct = default)
      where T : class, new()
    {
        if (typeof(T) == typeof(CarListingFacts))
        {
            if (ShouldFail)
            {
                return Task.FromResult(Result.Fail<ExtractionOutcome<T>>(
                    new ExtractionError("Simulated extraction failure")));
            }

            var facts = new CarListingFacts
            {
                Make = "Volkswagen",
                Model = "Golf",
                Year = 2018,
                MileageKm = 87200,
                Price = 42900,
                Currency = "PLN",
                Description = "Well-maintained Golf from pasted listing text."
            };

            return Task.FromResult(Result.Ok(new ExtractionOutcome<T>
            {
                Value = (T)(object)facts,
                Confidence = 0.95,
                Reasoning = null,
                RawJson = null,
                ModelId = "fake-model",
                InputTokens = 10,
                OutputTokens = 5
            }));
        }

        return Task.FromResult(Result.Fail<ExtractionOutcome<T>>(
            new ExtractionError($"Unsupported extraction type: {typeof(T).Name}")));
    }
}
