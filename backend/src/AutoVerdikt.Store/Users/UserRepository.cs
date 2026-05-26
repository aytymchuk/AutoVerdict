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

        return UserAccount.Reconstitute(
            document.Id,
            document.AuthId,
            document.Name,
            document.Email,
            new DateTimeOffset(document.RegisteredAt, TimeSpan.Zero));
    }

    public async Task<Result> CreateAsync(UserAccount user, CancellationToken cancellationToken = default)
    {
        var document = new UserDocument
        {
            Id = user.Id,
            AuthId = user.AuthId,
            Name = user.Name,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt.UtcDateTime
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
}
