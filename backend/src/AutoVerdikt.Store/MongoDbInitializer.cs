using AutoVerdikt.Store.Users;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace AutoVerdikt.Store;

internal sealed class MongoDbInitializer(IMongoCollection<UserDocument> users) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await users.Indexes.CreateOneAsync(
            new CreateIndexModel<UserDocument>(
                Builders<UserDocument>.IndexKeys.Ascending(u => u.Email),
                new CreateIndexOptions { Unique = true }),
            cancellationToken: cancellationToken);

        await users.Indexes.CreateOneAsync(
            new CreateIndexModel<UserDocument>(
                Builders<UserDocument>.IndexKeys.Ascending(u => u.AuthId),
                new CreateIndexOptions { Unique = true }),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
