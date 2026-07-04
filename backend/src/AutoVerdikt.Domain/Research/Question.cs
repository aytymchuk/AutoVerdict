namespace AutoVerdikt.Domain.Research;

public record Question
{
    public required Guid Id { get; init; }
    public required string QuestionText { get; init; }
    public string? Answer { get; init; }
    public bool ReviewedByAssistant { get; init; }

    public static Question Create(
        string questionText,
        TimeProvider timeProvider,
        string? answer = null,
        bool reviewedByAssistant = false)
    {
        var now = timeProvider.GetUtcNow();
        return new()
        {
            Id = Guid.CreateVersion7(now),
            QuestionText = questionText,
            Answer = answer,
            ReviewedByAssistant = reviewedByAssistant
        };
    }
}
