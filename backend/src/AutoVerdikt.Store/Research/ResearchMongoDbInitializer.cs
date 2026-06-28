using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Research;

internal sealed class ResearchMongoDbInitializer(IMongoCollection<ResearchDocument> collection) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await collection.Indexes.CreateOneAsync(
            new CreateIndexModel<ResearchDocument>(
                Builders<ResearchDocument>.IndexKeys
                    .Ascending(r => r.AuthId)
                    .Descending(r => r.UpdatedAt)),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
