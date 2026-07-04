namespace AutoVerdikt.Domain.Research;

public record Question
{
    public required string QuestionText { get; init; }
    public string? Answer { get; init; }
    public bool ReviewedByAssistant { get; init; }
}
