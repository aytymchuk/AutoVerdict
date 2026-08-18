using AutoVerdikt.Domain.Research;
using AutoVerdikt.Store.Research;
using Shouldly;

namespace AutoVerdikt.Store.Tests.Research;

public class QuestionMapperTests
{
    private static readonly Guid QuestionId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567893");
    private static readonly Guid ResearchId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567891");
    private const string AuthId = "auth_test";

    [Fact]
    public void ToDocument_PreservesId()
    {
        var question = new Question
        {
            Id = QuestionId,
            QuestionText = "Has the timing belt been replaced?",
            Answer = "Yes, at 80k km",
            ReviewedByAssistant = true
        };

        var document = QuestionMapper.ToDocument(question, ResearchId, AuthId);

        document.Id.ShouldBe(QuestionId);
    }

    [Fact]
    public void ToDocument_SetsResearchIdAndAuthId()
    {
        var question = new Question
        {
            Id = QuestionId,
            QuestionText = "Has the timing belt been replaced?",
            Answer = null,
            ReviewedByAssistant = false
        };

        var document = QuestionMapper.ToDocument(question, ResearchId, AuthId);

        document.ResearchId.ShouldBe(ResearchId);
        document.AuthId.ShouldBe(AuthId);
    }

    [Fact]
    public void ToDomain_RoundTripsAllFields()
    {
        var original = new Question
        {
            Id = QuestionId,
            QuestionText = "Has the timing belt been replaced?",
            Answer = "Yes, at 80k km",
            ReviewedByAssistant = true
        };

        var document = QuestionMapper.ToDocument(original, ResearchId, AuthId);
        var roundTripped = QuestionMapper.ToDomain(document);

        roundTripped.Id.ShouldBe(original.Id);
        roundTripped.QuestionText.ShouldBe(original.QuestionText);
        roundTripped.Answer.ShouldBe(original.Answer);
        roundTripped.ReviewedByAssistant.ShouldBe(original.ReviewedByAssistant);
    }
}
