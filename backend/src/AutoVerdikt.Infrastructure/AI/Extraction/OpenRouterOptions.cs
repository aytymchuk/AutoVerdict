namespace AutoVerdikt.Infrastructure.AI.Extraction;

public sealed class OpenRouterOptions
{
    public const string SectionName = "OpenRouter";
    public const string HttpClientName = "OpenRouterClient";

    public string ApiKey { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = "https://openrouter.ai/api/v1";
    public string ExtractionModel { get; init; } = "google/gemini-2.5-flash";
    public string SiteUrl { get; init; } = "https://autoverdikt.com";
    public string SiteName { get; init; } = "AutoVerdikt";
}
