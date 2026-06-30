using System.Reflection;
using AutoVerdikt.Application.AI.Extraction;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;

namespace AutoVerdikt.Infrastructure.AI.Extraction;

public sealed class ExtractionService(IChatClient chatClient, ILogger<ExtractionService> logger)
    : IExtractionService
{
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
                new(ChatRole.System, schemaAttr.SystemPrompt),
                new(ChatRole.User, input)
            };

            var options = new ChatOptions { Temperature = 0f };

            var response = await chatClient.GetResponseAsync<T>(
                messages,
                options,
                useJsonSchemaResponseFormat: true,
                cancellationToken: ct);

            var inputTokens = (int?)response.Usage?.InputTokenCount ?? 0;
            var outputTokens = (int?)response.Usage?.OutputTokenCount ?? 0;
            var rawJson = response.Text;

            return new ExtractionResult<T>(
                Value: response.Result,
                IsSuccess: response.Result is not null,
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
                RawJson: null,
                ModelId: "unknown",
                InputTokens: 0,
                OutputTokens: 0,
                ErrorMessage: ex.Message);
        }
    }
}
