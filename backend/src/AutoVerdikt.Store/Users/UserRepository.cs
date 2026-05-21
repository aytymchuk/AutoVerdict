using AutoVerdikt.Application.Users;
using AutoVerdikt.Domain.Users;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Users;

internal sealed class UserRepository(IMongoCollection<UserDocument> collection) : IUserRepository
{
    public async Task<bool> ExistsByAuthIdAsync(string clerkId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserDocument>.Filter.Eq(u => u.AuthId, clerkId);
        return await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    public async Task CreateAsync(UserAccount user, CancellationToken cancellationToken = default)
    {
        var document = new UserDocument
        {
            Id = user.Id,
            AuthId = user.AuthId,
            Name = user.Name,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt.UtcDateTime
        };
        await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }
}
