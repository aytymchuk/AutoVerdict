using AutoVerdikt.Domain.Research;
using Shouldly;

namespace AutoVerdikt.Domain.Tests.Research;

public class NoteTests
{
    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    [Fact]
    public void Create_SetsAllPropertiesFromInputs()
    {
        var date = new DateTimeOffset(2026, 3, 15, 10, 30, 0, TimeSpan.Zero);
        var timeProvider = new FixedTimeProvider(date);

        var note = Note.Create(date, "Called the seller", timeProvider);

        note.Date.ShouldBe(date);
        note.Text.ShouldBe("Called the seller");
    }

    [Fact]
    public void Create_AssignsUuidV7Id()
    {
        var note = Note.Create(
            new DateTimeOffset(2026, 3, 15, 10, 30, 0, TimeSpan.Zero),
            "Called the seller",
            TimeProvider.System);

        note.Id.ToString()[14].ShouldBe('7');
    }

    [Fact]
    public void Create_IdReflectsCreationTimestamp()
    {
        var later = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);
        var earlier = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var laterNote = Note.Create(later, "Later note", new FixedTimeProvider(later));
        var earlierNote = Note.Create(earlier, "Earlier note", new FixedTimeProvider(earlier));

        laterNote.Id.ShouldBeGreaterThan(earlierNote.Id);
    }
}
