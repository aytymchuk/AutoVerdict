using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Domain.Whitelist;
using AutoVerdikt.Store.Waitlist;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Whitelist;

internal sealed class WhitelistRepository(IMongoCollection<WaitlistRequestDocument> collection) : IWhitelistRepository
{
    private const string ApprovedStatus = "approved";

    public async Task<bool> ExistsByAuthIdAsync(string authId, CancellationToken cancellationToken = default)
    {
        var filter = ApprovedByAuthIdFilter(authId);
        return await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    public async Task AddAsync(WhitelistEntry entry, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WaitlistRequestDocument>.Filter.Eq(w => w.AuthId, entry.AuthId);
        var existing = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        var now = entry.CreatedAt.UtcDateTime;

        var document = new WaitlistRequestDocument
        {
            Id = existing?.Id ?? entry.Id,
            AuthId = entry.AuthId,
            Email = entry.Email,
            About = existing?.About,
            Status = ApprovedStatus,
            Locale = existing?.Locale ?? "en",
            CreatedAt = existing?.CreatedAt ?? now,
            UpdatedAt = now,
            ReviewedBy = entry.AddedBy ?? existing?.ReviewedBy,
            ReviewedAt = existing?.ReviewedAt ?? now
        };

        try
        {
            await collection.ReplaceOneAsync(
                filter,
                document,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Code == 11000)
        {
            // idempotent — duplicate auth_id
        }
    }

    public async Task<bool> RemoveByAuthIdAsync(string authId, CancellationToken cancellationToken = default)
    {
        var filter = ApprovedByAuthIdFilter(authId);
        var result = await collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<PaginatedResult<WhitelistEntry>> ListAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<WaitlistRequestDocument>.Filter.Eq(w => w.Status, ApprovedStatus);
        var skip = Math.Max(0, (page - 1) * pageSize);
        var total = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var documents = await collection
            .Find(filter)
            .SortByDescending(w => w.CreatedAt)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        var items = documents.Select(Map).ToList();
        return new PaginatedResult<WhitelistEntry>(items, total);
    }

    private static FilterDefinition<WaitlistRequestDocument> ApprovedByAuthIdFilter(string authId) =>
        Builders<WaitlistRequestDocument>.Filter.And(
            Builders<WaitlistRequestDocument>.Filter.Eq(w => w.AuthId, authId),
            Builders<WaitlistRequestDocument>.Filter.Eq(w => w.Status, ApprovedStatus));

    private static WhitelistEntry Map(WaitlistRequestDocument document) =>
        new()
        {
            Id = document.Id,
            AuthId = document.AuthId,
            Email = document.Email,
            CreatedAt = new DateTimeOffset(document.CreatedAt, TimeSpan.Zero),
            AddedBy = document.ReviewedBy
        };
}
