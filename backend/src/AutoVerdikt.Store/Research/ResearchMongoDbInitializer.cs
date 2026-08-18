using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Research;

internal sealed class ResearchMongoDbInitializer(
    IMongoCollection<ResearchDocument> researchCollection,
    IMongoCollection<NoteDocument> notesCollection,
    IMongoCollection<DetailDocument> detailsCollection,
    IMongoCollection<AttachedFileDocument> filesCollection,
    IMongoCollection<QuestionDocument> questionsCollection,
    IMongoCollection<BadgeDocument> badgesCollection) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await researchCollection.Indexes.CreateOneAsync(
            new CreateIndexModel<ResearchDocument>(
                Builders<ResearchDocument>.IndexKeys
                    .Ascending(r => r.AuthId)
                    .Descending(r => r.UpdatedAt)),
            cancellationToken: cancellationToken);

        await CreateChildIndexAsync(notesCollection, cancellationToken);
        await CreateChildIndexAsync(detailsCollection, cancellationToken);
        await CreateChildIndexAsync(filesCollection, cancellationToken);
        await CreateChildIndexAsync(questionsCollection, cancellationToken);
        await CreateChildIndexAsync(badgesCollection, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task CreateChildIndexAsync<T>(
        IMongoCollection<T> collection,
        CancellationToken cancellationToken)
        where T : ResearchChildDocument
    {
        await collection.Indexes.CreateOneAsync(
            new CreateIndexModel<T>(
                Builders<T>.IndexKeys
                    .Ascending(d => d.ResearchId)
                    .Ascending(d => d.AuthId)),
            cancellationToken: cancellationToken);
    }
}
