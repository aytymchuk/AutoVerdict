using System.Reflection;
using AutoVerdikt.Application.AI.Extraction;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;

namespace AutoVerdikt.Infrastructure.AI.Extraction;

public sealed class ExtractionService(IChatClient chatClient, ILogger<ExtractionService> logger)
    : IExtractionService
{
    private const string ConfidenceInstruction = """
        Also assess how well the input matches the expected domain described above.
        Set confidence to a value between 0.0 and 1.0 (1.0 = clearly on-topic and extraction is accurate).
        Set confidence below 0.75 when the input is unrelated to the expected domain or too ambiguous to extract reliably.
        Provide brief reasoning explaining the confidence score.
        """;

    public async Task<ExtractionResult<T>> ExtractAsync<T>(
        string input,
        CancellationToken ct = default)
        where T : class, new()
    {
        var schemaAttr = typeof(T).GetCustomAttribute<ExtractionSchemaAttribute>()
            ?? throw new InvalidOperationException(
                $"{typeof(T).Name} must be decorated with [ExtractionSchema]");

        try
        {
            var messages = new List<ChatMessage>
            {
                new(ChatRole.System, $"{schemaAttr.SystemPrompt}\n\n{ConfidenceInstruction}"),
                new(ChatRole.User, input)
            };

            var options = new ChatOptions { Temperature = 0f };

            var response = await chatClient.GetResponseAsync<ExtractionEnvelope<T>>(
                messages,
                options,
                useJsonSchemaResponseFormat: true,
                cancellationToken: ct);

            var inputTokens = (int?)response.Usage?.InputTokenCount ?? 0;
            var outputTokens = (int?)response.Usage?.OutputTokenCount ?? 0;
            var rawJson = response.Text;
            var envelope = response.Result;

            return new ExtractionResult<T>(
                Value: envelope?.Data,
                IsSuccess: envelope is not null,
                Confidence: envelope?.Confidence ?? 0,
                Reasoning: envelope?.Reasoning,
                RawJson: rawJson,
                ModelId: response.ModelId ?? "unknown",
                InputTokens: inputTokens,
                OutputTokens: outputTokens);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Extraction failed for {ExtractionType}", typeof(T).Name);
            return new ExtractionResult<T>(
                Value: null,
                IsSuccess: false,
                Confidence: 0,
                Reasoning: null,
                RawJson: null,
                ModelId: "unknown",
                InputTokens: 0,
                OutputTokens: 0,
                ErrorMessage: ex.Message);
        }
    }
}
