using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Store.Research;

internal static class NoteMapper
{
    public static NoteDocument ToDocument(Note note, Guid researchId, string authId) =>
        new()
        {
            Id = note.Id,
            ResearchId = researchId,
            AuthId = authId,
            Date = note.Date.UtcDateTime,
            Text = note.Text
        };

    public static Note ToDomain(NoteDocument document) =>
        new()
        {
            Id = document.Id,
            Date = new DateTimeOffset(document.Date, TimeSpan.Zero),
            Text = document.Text
        };
}
