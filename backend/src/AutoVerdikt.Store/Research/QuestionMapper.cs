using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Store.Research;

internal static class QuestionMapper
{
    public static QuestionDocument ToDocument(Question question, Guid researchId, string authId) =>
        new()
        {
            Id = question.Id,
            ResearchId = researchId,
            AuthId = authId,
            QuestionText = question.QuestionText,
            Answer = question.Answer,
            ReviewedByAssistant = question.ReviewedByAssistant
        };

    public static Question ToDomain(QuestionDocument document) =>
        new()
        {
            Id = document.Id,
            QuestionText = document.QuestionText,
            Answer = document.Answer,
            ReviewedByAssistant = document.ReviewedByAssistant
        };
}
