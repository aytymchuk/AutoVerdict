using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Domain.Whitelist;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Waitlist;

internal sealed class WaitlistRequestRepository(IMongoCollection<WaitlistRequestDocument> collection)
    : IWaitlistRequestRepository
{
    public async Task<bool> ExistsByAuthIdAsync(string authId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WaitlistRequestDocument>.Filter.Eq(w => w.AuthId, authId);
        return await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WaitlistRequestDocument>.Filter.Eq(w => w.Email, email);
        return await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    public async Task CreateAsync(WaitlistRequest request, CancellationToken cancellationToken = default)
    {
        var document = MapToDocument(request);
        await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task<WaitlistRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WaitlistRequestDocument>.Filter.Eq(w => w.Id, id);
        var document = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task UpdateAsync(WaitlistRequest request, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WaitlistRequestDocument>.Filter.Eq(w => w.Id, request.Id);
        await collection.ReplaceOneAsync(filter, MapToDocument(request), cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<WaitlistRequest>> ListByStatusAsync(
        WaitlistRequestStatus? status,
        CancellationToken cancellationToken = default)
    {
        FilterDefinition<WaitlistRequestDocument> filter = FilterDefinition<WaitlistRequestDocument>.Empty;
        if (status is not null)
            filter = Builders<WaitlistRequestDocument>.Filter.Eq(w => w.Status, ToStatusString(status.Value));

        var documents = await collection
            .Find(filter)
            .SortByDescending(w => w.CreatedAt)
            .ToListAsync(cancellationToken);

        return documents.Select(Map).ToList();
    }

    private static WaitlistRequestDocument MapToDocument(WaitlistRequest request) => new()
    {
        Id = request.Id,
        AuthId = request.AuthId,
        Email = request.Email,
        About = request.About,
        Status = ToStatusString(request.Status),
        Locale = request.Locale,
        CreatedAt = request.CreatedAt.UtcDateTime,
        UpdatedAt = request.UpdatedAt.UtcDateTime,
        ReviewedBy = request.ReviewedBy,
        ReviewedAt = request.ReviewedAt?.UtcDateTime
    };

    private static WaitlistRequest Map(WaitlistRequestDocument document) =>
        new()
        {
            Id = document.Id,
            AuthId = document.AuthId,
            Email = document.Email,
            About = document.About,
            Status = ParseStatus(document.Status),
            Locale = document.Locale,
            CreatedAt = new DateTimeOffset(document.CreatedAt, TimeSpan.Zero),
            UpdatedAt = new DateTimeOffset(document.UpdatedAt, TimeSpan.Zero),
            ReviewedBy = document.ReviewedBy,
            ReviewedAt = document.ReviewedAt is null ? null : new DateTimeOffset(document.ReviewedAt.Value, TimeSpan.Zero)
        };

    private static string ToStatusString(WaitlistRequestStatus status) => status switch
    {
        WaitlistRequestStatus.Pending => "pending",
        WaitlistRequestStatus.Approved => "approved",
        WaitlistRequestStatus.Rejected => "rejected",
        _ => "pending"
    };

    private static WaitlistRequestStatus ParseStatus(string status) => status switch
    {
        "approved" => WaitlistRequestStatus.Approved,
        "rejected" => WaitlistRequestStatus.Rejected,
        _ => WaitlistRequestStatus.Pending
    };
}
