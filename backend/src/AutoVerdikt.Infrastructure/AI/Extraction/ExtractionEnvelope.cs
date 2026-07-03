using System.ComponentModel;

namespace AutoVerdikt.Infrastructure.AI.Extraction;

internal sealed class ExtractionEnvelope<T> where T : class, new()
{
    [Description("Confidence 0.0-1.0 that the input matches the expected domain and the extraction is accurate.")]
    public double Confidence { get; init; }

    [Description("Brief reasoning for the confidence score; explain why if the input seems unrelated to the expected domain.")]
    public string? Reasoning { get; init; }

    [Description("Extracted data. Populate best-effort even when confidence is low.")]
    public T? Data { get; init; }
}
