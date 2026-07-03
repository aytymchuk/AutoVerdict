using AutoVerdikt.Application.AI.Extraction;
using AutoVerdikt.Application.Research.Create.StartText;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public sealed class FakeExtractionService : IExtractionService
{
    public Task<ExtractionResult<T>> ExtractAsync<T>(string input, CancellationToken ct = default)
      where T : class, new()
    {
        if (typeof(T) == typeof(CarListingFacts))
        {
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

            return Task.FromResult(new ExtractionResult<T>(
              Value: (T)(object)facts,
              IsSuccess: true,
              Confidence: 0.95,
              Reasoning: null,
              RawJson: null,
              ModelId: "fake-model",
              InputTokens: 10,
              OutputTokens: 5));
        }

        return Task.FromResult(new ExtractionResult<T>(
          Value: null,
          IsSuccess: false,
          Confidence: 0,
          Reasoning: null,
          RawJson: null,
          ModelId: "fake-model",
          InputTokens: 0,
          OutputTokens: 0,
          ErrorMessage: $"Unsupported extraction type: {typeof(T).Name}"));
    }
}
