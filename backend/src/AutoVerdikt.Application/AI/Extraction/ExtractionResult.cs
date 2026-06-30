namespace AutoVerdikt.Application.AI.Extraction;

public sealed record ExtractionResult<T>(
    T? Value,
    bool IsSuccess,
    string? RawJson,
    string ModelId,
    int InputTokens,
    int OutputTokens,
    string? ErrorMessage = null);
