namespace AutoVerdikt.Application.AI.Extraction;

public interface IExtractionService
{
    Task<Result<ExtractionOutcome<T>>> ExtractAsync<T>(
        string input,
        CancellationToken ct = default)
        where T : class, new();
}
