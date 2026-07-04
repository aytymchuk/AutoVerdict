using MongoDB.Driver;

namespace AutoVerdikt.Store.Research;

internal static class ResearchChildFilters
{
    public static FilterDefinition<T> ByResearchIdAndAuthId<T>(Guid researchId, string authId)
        where T : ResearchChildDocument =>
        Builders<T>.Filter.And(
            Builders<T>.Filter.Eq(d => d.ResearchId, researchId),
            Builders<T>.Filter.Eq(d => d.AuthId, authId));

    public static FilterDefinition<NoteDocument> Notes(Guid researchId, string authId) =>
        ByResearchIdAndAuthId<NoteDocument>(researchId, authId);

    public static FilterDefinition<DetailDocument> Details(Guid researchId, string authId) =>
        ByResearchIdAndAuthId<DetailDocument>(researchId, authId);

    public static FilterDefinition<AttachedFileDocument> Files(Guid researchId, string authId) =>
        ByResearchIdAndAuthId<AttachedFileDocument>(researchId, authId);

    public static FilterDefinition<QuestionDocument> Questions(Guid researchId, string authId) =>
        ByResearchIdAndAuthId<QuestionDocument>(researchId, authId);

    public static FilterDefinition<BadgeDocument> Badges(Guid researchId, string authId) =>
        ByResearchIdAndAuthId<BadgeDocument>(researchId, authId);
}
