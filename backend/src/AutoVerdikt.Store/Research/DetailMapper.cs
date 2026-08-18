using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Store.Research;

internal static class DetailMapper
{
    public static DetailDocument ToDocument(Detail detail, Guid researchId, string authId) =>
        new()
        {
            Id = detail.Id,
            ResearchId = researchId,
            AuthId = authId,
            Name = detail.Name,
            Value = detail.Value
        };

    public static Detail ToDomain(DetailDocument document) =>
        new()
        {
            Id = document.Id,
            Name = document.Name,
            Value = document.Value
        };
}
