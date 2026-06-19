using FluentResults;
using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Users.Errors;
using AutoVerdikt.Domain.Users;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Users;

internal sealed class UserRepository(IMongoCollection<UserDocument> collection) : IUserRepository
{
    public async Task<bool> ExistsByAuthIdAsync(string authId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserDocument>.Filter.Eq(u => u.AuthId, authId);
        return await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    public async Task<UserAccount?> GetByAuthIdAsync(string authId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserDocument>.Filter.Eq(u => u.AuthId, authId);
        var document = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if (document is null)
            return null;

        return new UserAccount
        {
            Id = document.Id,
            AuthId = document.AuthId,
            Name = document.Name,
            Email = document.Email,
            RegisteredAt = new DateTimeOffset(document.RegisteredAt, TimeSpan.Zero),
            WhitelistStatus = ParseWhitelistStatus(document.WhitelistStatus ?? document.LegacyWaitlistStatus)
        };
    }

    public async Task<Result> CreateAsync(UserAccount user, CancellationToken cancellationToken = default)
    {
        var document = new UserDocument
        {
            Id = user.Id,
            AuthId = user.AuthId,
            Name = user.Name,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt.UtcDateTime,
            WhitelistStatus = ToWhitelistStatusString(user.WhitelistStatus)
        };
        try
        {
            await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
            return Result.Ok();
        }
        catch (MongoWriteException ex) when (ex.WriteError.Code == 11000)
        {
            return Result.Fail(new UserAlreadyRegisteredError());
        }
    }

    public async Task UpdateWhitelistStatusAsync(
        UserAccount user,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserDocument>.Filter.Eq(u => u.AuthId, user.AuthId);
        var update = Builders<UserDocument>.Update.Set(
            u => u.WhitelistStatus,
            ToWhitelistStatusString(user.WhitelistStatus));

        await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    private static string ToWhitelistStatusString(WhitelistStatus status) => status switch
    {
        WhitelistStatus.None => "none",
        WhitelistStatus.Requested => "requested",
        WhitelistStatus.Approved => "approved",
        WhitelistStatus.Declined => "declined",
        _ => "none"
    };

    private static WhitelistStatus ParseWhitelistStatus(string? status) => status switch
    {
        "none" => WhitelistStatus.None,
        "requested" => WhitelistStatus.Requested,
        "approved" => WhitelistStatus.Approved,
        "declined" => WhitelistStatus.Declined,
        // Legacy values from waitlistStatus field
        "pending" => WhitelistStatus.Requested,
        "rejected" => WhitelistStatus.Declined,
        null => WhitelistStatus.None,
        _ => WhitelistStatus.None
    };
}
