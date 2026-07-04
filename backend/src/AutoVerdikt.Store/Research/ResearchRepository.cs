using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.Research;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Domain.Research;
using FluentResults;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Research;

internal sealed class ResearchRepository(
    IMongoClient mongoClient,
    IMongoCollection<ResearchDocument> researchCollection,
    IMongoCollection<NoteDocument> notesCollection,
    IMongoCollection<DetailDocument> detailsCollection,
    IMongoCollection<AttachedFileDocument> filesCollection,
    IMongoCollection<QuestionDocument> questionsCollection,
    IMongoCollection<BadgeDocument> badgesCollection) : IResearchRepository
{
    private static readonly TransactionOptions TransactionOptions = new(
        readConcern: ReadConcern.Snapshot,
        writeConcern: WriteConcern.WMajority);

    public async Task<Result> CreateAsync(ResearchRecord record, CancellationToken cancellationToken = default)
    {
        try
        {
            using var session = await mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            await session.WithTransactionAsync(
                async (s, ct) =>
                {
                    await researchCollection.InsertOneAsync(
                        s,
                        ResearchDocumentMapper.ToDocument(record),
                        cancellationToken: ct);
                    await InsertChildDocumentsAsync(s, record, ct);
                    return true;
                },
                TransactionOptions,
                cancellationToken);
            return Result.Ok();
        }
        catch (MongoException)
        {
            return Result.Fail(new Error("Failed to create research record."));
        }
    }

    public async Task<Result<ResearchRecord?>> GetByIdAndAuthIdAsync(
        Guid id,
        string authId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ResearchDocument>.Filter.And(
                Builders<ResearchDocument>.Filter.Eq(r => r.Id, id),
                Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId));

            var document = await researchCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            if (document is null)
                return Result.Ok<ResearchRecord?>(null);

            var notes = await notesCollection
                .Find(ResearchChildFilters.Notes(id, authId))
                .ToListAsync(cancellationToken);
            var details = await detailsCollection
                .Find(ResearchChildFilters.Details(id, authId))
                .ToListAsync(cancellationToken);
            var files = await filesCollection
                .Find(ResearchChildFilters.Files(id, authId))
                .ToListAsync(cancellationToken);
            var questions = await questionsCollection
                .Find(ResearchChildFilters.Questions(id, authId))
                .ToListAsync(cancellationToken);
            var badges = await badgesCollection
                .Find(ResearchChildFilters.Badges(id, authId))
                .ToListAsync(cancellationToken);

            return Result.Ok<ResearchRecord?>(ResearchDocumentMapper.ToDomain(
                document,
                notes.Select(NoteMapper.ToDomain).ToList(),
                details.Select(DetailMapper.ToDomain).ToList(),
                files.Select(AttachedFileMapper.ToDomain).ToList(),
                questions.Select(QuestionMapper.ToDomain).ToList(),
                badges.Select(BadgeMapper.ToDomain).ToList()));
        }
        catch (MongoException)
        {
            return Result.Fail<ResearchRecord?>(new Error("Failed to load research record."));
        }
    }

    public async Task<Result> UpdateAsync(
        ResearchRecord record,
        string authId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var session = await mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            var updated = await session.WithTransactionAsync(
                async (s, ct) =>
                {
                    var filter = Builders<ResearchDocument>.Filter.And(
                        Builders<ResearchDocument>.Filter.Eq(r => r.Id, record.Id),
                        Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId));

                    var result = await researchCollection.ReplaceOneAsync(
                        s,
                        filter,
                        ResearchDocumentMapper.ToDocument(record),
                        cancellationToken: ct);

                    if (result.MatchedCount == 0)
                        return false;

                    await ReplaceChildDocumentsAsync(s, record, authId, ct);
                    return true;
                },
                TransactionOptions,
                cancellationToken);

            if (!updated)
                return Result.Fail(new ResearchNotFoundError());

            return Result.Ok();
        }
        catch (MongoException)
        {
            return Result.Fail(new Error("Failed to update research record."));
        }
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        string authId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var session = await mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            var deleted = await session.WithTransactionAsync(
                async (s, ct) =>
                {
                    var filter = Builders<ResearchDocument>.Filter.And(
                        Builders<ResearchDocument>.Filter.Eq(r => r.Id, id),
                        Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId));

                    var result = await researchCollection.DeleteOneAsync(s, filter, cancellationToken: ct);
                    if (result.DeletedCount == 0)
                        return false;

                    await DeleteChildDocumentsAsync(s, id, authId, ct);
                    return true;
                },
                TransactionOptions,
                cancellationToken);

            if (!deleted)
                return Result.Fail(new ResearchNotFoundError());

            return Result.Ok();
        }
        catch (MongoException)
        {
            return Result.Fail(new Error("Failed to delete research record."));
        }
    }

    // List queries only the core research collection; child lists are not loaded.
    public async Task<Result<PaginatedResult<ResearchRecord>>> ListByAuthIdAsync(
        string authId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId);
            var skip = Math.Max(0, (page - 1) * pageSize);
            var total = await researchCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var documents = await researchCollection
                .Find(filter)
                .SortByDescending(r => r.UpdatedAt)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            var items = documents.Select(ResearchDocumentMapper.ToDomainListItem).ToList();
            return Result.Ok(new PaginatedResult<ResearchRecord>(items, total));
        }
        catch (MongoException)
        {
            return Result.Fail<PaginatedResult<ResearchRecord>>(new Error("Failed to list research records."));
        }
    }

    private async Task ReplaceChildDocumentsAsync(
        IClientSessionHandle session,
        ResearchRecord record,
        string authId,
        CancellationToken cancellationToken)
    {
        await DeleteChildDocumentsAsync(session, record.Id, authId, cancellationToken);
        await InsertChildDocumentsAsync(session, record, cancellationToken);
    }

    private async Task InsertChildDocumentsAsync(
        IClientSessionHandle session,
        ResearchRecord record,
        CancellationToken cancellationToken)
    {
        if (record.Notes.Count > 0)
        {
            await notesCollection.InsertManyAsync(
                session,
                record.Notes.Select(n => NoteMapper.ToDocument(n, record.Id, record.AuthId)),
                cancellationToken: cancellationToken);
        }

        if (record.Details.Count > 0)
        {
            await detailsCollection.InsertManyAsync(
                session,
                record.Details.Select(d => DetailMapper.ToDocument(d, record.Id, record.AuthId)),
                cancellationToken: cancellationToken);
        }

        if (record.Files.Count > 0)
        {
            await filesCollection.InsertManyAsync(
                session,
                record.Files.Select(f => AttachedFileMapper.ToDocument(f, record.Id, record.AuthId)),
                cancellationToken: cancellationToken);
        }

        if (record.Questions.Count > 0)
        {
            await questionsCollection.InsertManyAsync(
                session,
                record.Questions.Select(q => QuestionMapper.ToDocument(q, record.Id, record.AuthId)),
                cancellationToken: cancellationToken);
        }

        if (record.Badges.Count > 0)
        {
            await badgesCollection.InsertManyAsync(
                session,
                record.Badges.Select(b => BadgeMapper.ToDocument(b, record.Id, record.AuthId)),
                cancellationToken: cancellationToken);
        }
    }

    private async Task DeleteChildDocumentsAsync(
        IClientSessionHandle session,
        Guid researchId,
        string authId,
        CancellationToken cancellationToken)
    {
        await notesCollection.DeleteManyAsync(
            session,
            ResearchChildFilters.Notes(researchId, authId),
            cancellationToken: cancellationToken);
        await detailsCollection.DeleteManyAsync(
            session,
            ResearchChildFilters.Details(researchId, authId),
            cancellationToken: cancellationToken);
        await filesCollection.DeleteManyAsync(
            session,
            ResearchChildFilters.Files(researchId, authId),
            cancellationToken: cancellationToken);
        await questionsCollection.DeleteManyAsync(
            session,
            ResearchChildFilters.Questions(researchId, authId),
            cancellationToken: cancellationToken);
        await badgesCollection.DeleteManyAsync(
            session,
            ResearchChildFilters.Badges(researchId, authId),
            cancellationToken: cancellationToken);
    }
}
