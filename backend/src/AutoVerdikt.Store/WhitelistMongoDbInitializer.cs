using AutoVerdikt.Store.Users;
using AutoVerdikt.Store.Waitlist;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace AutoVerdikt.Store;

internal sealed class WhitelistMongoDbInitializer(
    IMongoCollection<UserDocument> users,
    IMongoCollection<WaitlistRequestDocument> waitlistRequests) : IHostedService
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

        await waitlistRequests.Indexes.CreateOneAsync(
            new CreateIndexModel<WaitlistRequestDocument>(
                Builders<WaitlistRequestDocument>.IndexKeys.Ascending(w => w.AuthId),
                new CreateIndexOptions { Unique = true }),
            cancellationToken: cancellationToken);

        await waitlistRequests.Indexes.CreateOneAsync(
            new CreateIndexModel<WaitlistRequestDocument>(
                Builders<WaitlistRequestDocument>.IndexKeys.Ascending(w => w.Email),
                new CreateIndexOptions { Unique = true }),
            cancellationToken: cancellationToken);

        await waitlistRequests.Indexes.CreateOneAsync(
            new CreateIndexModel<WaitlistRequestDocument>(
                Builders<WaitlistRequestDocument>.IndexKeys.Ascending(w => w.Status)),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
