namespace AutoVerdikt.Application.AI.Extraction;

public sealed record ExtractionResult<T>(
    T? Value,
    bool IsSuccess,
    double Confidence,
    string? Reasoning,
    string? RawJson,
    string ModelId,
    int InputTokens,
    int OutputTokens,
    string? ErrorMessage = null)
{
    public const double MinConfidenceThreshold = 0.75;

    public bool IsIntentMatch => Confidence >= MinConfidenceThreshold;
}
