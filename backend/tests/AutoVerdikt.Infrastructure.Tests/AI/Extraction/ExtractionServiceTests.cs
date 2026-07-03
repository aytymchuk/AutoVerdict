using System.ComponentModel;
using AutoVerdikt.Application.Research.Create.StartText;
using AutoVerdikt.Infrastructure.AI.Extraction;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;

namespace AutoVerdikt.Infrastructure.Tests.AI.Extraction;

public sealed class ExtractionServiceTests
{
    private readonly Mock<IChatClient> _chatClient = new();

    [Fact]
    public async Task ExtractAsync_ReturnsDeserializedResult_WhenChatClientSucceeds()
    {
        var json = """
            {
              "confidence": 0.92,
              "reasoning": "Clear used car listing with make, model, year, mileage, and price.",
              "data": {
                "make": "Volkswagen",
                "model": "Golf",
                "year": 2018,
                "mileageKm": 87200,
                "price": 42900,
                "currency": "PLN"
              }
            }
            """;
        var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, json))
        {
            ModelId = "google/gemini-2.5-flash",
            Usage = new UsageDetails { InputTokenCount = 100, OutputTokenCount = 50 }
        };

        _chatClient
          .Setup(c => c.GetResponseAsync(
            It.IsAny<IEnumerable<ChatMessage>>(),
            It.IsAny<ChatOptions>(),
            It.IsAny<CancellationToken>()))
          .ReturnsAsync(response);

        var service = new ExtractionService(_chatClient.Object, NullLogger<ExtractionService>.Instance);

        var result = await service.ExtractAsync<CarListingFacts>("VW Golf 2018, 87k km, 42900 PLN");

        result.IsSuccess.ShouldBeTrue();
        result.IsIntentMatch.ShouldBeTrue();
        result.Confidence.ShouldBe(0.92);
        result.Reasoning.ShouldBe("Clear used car listing with make, model, year, mileage, and price.");
        result.Value!.Make.ShouldBe("Volkswagen");
        result.Value.Model.ShouldBe("Golf");
        result.Value.Year.ShouldBe(2018);
        result.ModelId.ShouldBe("google/gemini-2.5-flash");
        result.InputTokens.ShouldBe(100);
        result.OutputTokens.ShouldBe(50);
    }

    [Fact]
    public async Task ExtractAsync_ReturnsFailure_WhenChatClientThrows()
    {
        _chatClient
          .Setup(c => c.GetResponseAsync(
            It.IsAny<IEnumerable<ChatMessage>>(),
            It.IsAny<ChatOptions>(),
            It.IsAny<CancellationToken>()))
          .ThrowsAsync(new InvalidOperationException("LLM unavailable"));

        var service = new ExtractionService(_chatClient.Object, NullLogger<ExtractionService>.Instance);

        var result = await service.ExtractAsync<CarListingFacts>("some text");

        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("LLM unavailable");
        result.Value.ShouldBeNull();
    }

    [Fact]
    public async Task ExtractAsync_ThrowsInvalidOperationException_WhenSchemaAttributeMissing()
    {
        var service = new ExtractionService(_chatClient.Object, NullLogger<ExtractionService>.Instance);

        await Should.ThrowAsync<InvalidOperationException>(
          () => service.ExtractAsync<UnschemaedFacts>("some text"));
    }

    [Fact]
    public async Task ExtractAsync_SetsTemperatureToZero()
    {
        ChatOptions? capturedOptions = null;
        var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, """{"confidence":0.9,"data":{"make":"BMW"}}"""));

        _chatClient
          .Setup(c => c.GetResponseAsync(
            It.IsAny<IEnumerable<ChatMessage>>(),
            It.IsAny<ChatOptions>(),
            It.IsAny<CancellationToken>()))
          .Callback<IEnumerable<ChatMessage>, ChatOptions?, CancellationToken>((_, options, _) => capturedOptions = options)
          .ReturnsAsync(response);

        var service = new ExtractionService(_chatClient.Object, NullLogger<ExtractionService>.Instance);
        await service.ExtractAsync<CarListingFacts>("BMW 320d");

        capturedOptions.ShouldNotBeNull();
        capturedOptions!.Temperature.ShouldBe(0f);
    }

    [Description("Test type without extraction schema.")]
    private sealed class UnschemaedFacts
    {
        [Description("Name")]
        public string? Name { get; init; }
    }
}
