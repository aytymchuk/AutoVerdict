namespace AutoVerdikt.Application.AI.Extraction;

public sealed class ExtractionOutcome<T> where T : class
{
    public const double MinConfidenceThreshold = 0.75;

    public T? Value { get; init; }
    public double Confidence { get; init; }
    public string? Reasoning { get; init; }
    public string? RawJson { get; init; }
    public string ModelId { get; init; } = string.Empty;
    public int InputTokens { get; init; }
    public int OutputTokens { get; init; }

    public bool IsIntentMatch => Confidence >= MinConfidenceThreshold;
}
