using AutoVerdikt.Domain.Research;
using Shouldly;

namespace AutoVerdikt.Domain.Tests.Research;

public class QuestionTests
{
    [Fact]
    public void Create_SetsRequiredFieldsAndDefaults()
    {
        var question = Question.Create("Has the timing belt been replaced?", TimeProvider.System);

        question.QuestionText.ShouldBe("Has the timing belt been replaced?");
        question.Answer.ShouldBeNull();
        question.ReviewedByAssistant.ShouldBeFalse();
    }

    [Fact]
    public void Create_WithAnswerAndReviewedFlag_SetsOptionalFields()
    {
        var question = Question.Create(
            "Has the timing belt been replaced?",
            TimeProvider.System,
            answer: "Yes, at 80k km",
            reviewedByAssistant: true);

        question.Answer.ShouldBe("Yes, at 80k km");
        question.ReviewedByAssistant.ShouldBeTrue();
    }

    [Fact]
    public void Create_AssignsUuidV7Id()
    {
        var question = Question.Create("Has the timing belt been replaced?", TimeProvider.System);

        question.Id.ToString()[14].ShouldBe('7');
    }
}
