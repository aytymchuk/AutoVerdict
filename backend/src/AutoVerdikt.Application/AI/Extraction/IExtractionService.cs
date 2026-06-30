namespace AutoVerdikt.Application.AI.Extraction;

public interface IExtractionService
{
    Task<ExtractionResult<T>> ExtractAsync<T>(
        string input,
        CancellationToken ct = default)
        where T : class, new();
}
