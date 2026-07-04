using AutoVerdikt.Domain.Research;
using AutoVerdikt.Store.Research;
using Shouldly;

namespace AutoVerdikt.Store.Tests.Research;

public class NoteMapperTests
{
    private static readonly Guid NoteId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567890");
    private static readonly Guid ResearchId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567891");
    private const string AuthId = "auth_test";

    [Fact]
    public void ToDocument_PreservesId()
    {
        var note = new Note
        {
            Id = NoteId,
            Date = new DateTimeOffset(2026, 3, 15, 10, 30, 0, TimeSpan.Zero),
            Text = "Called the seller"
        };

        var document = NoteMapper.ToDocument(note, ResearchId, AuthId);

        document.Id.ShouldBe(NoteId);
    }

    [Fact]
    public void ToDocument_SetsResearchIdAndAuthId()
    {
        var note = new Note
        {
            Id = NoteId,
            Date = new DateTimeOffset(2026, 3, 15, 10, 30, 0, TimeSpan.Zero),
            Text = "Called the seller"
        };

        var document = NoteMapper.ToDocument(note, ResearchId, AuthId);

        document.ResearchId.ShouldBe(ResearchId);
        document.AuthId.ShouldBe(AuthId);
    }

    [Fact]
    public void ToDomain_RoundTripsAllFields()
    {
        var original = new Note
        {
            Id = NoteId,
            Date = new DateTimeOffset(2026, 3, 15, 10, 30, 0, TimeSpan.Zero),
            Text = "Called the seller"
        };

        var document = NoteMapper.ToDocument(original, ResearchId, AuthId);
        var roundTripped = NoteMapper.ToDomain(document);

        roundTripped.Id.ShouldBe(original.Id);
        roundTripped.Date.ShouldBe(original.Date);
        roundTripped.Text.ShouldBe(original.Text);
    }
}
