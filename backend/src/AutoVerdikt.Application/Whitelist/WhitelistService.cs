using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Application.Identity;
using AutoVerdikt.Domain.Whitelist;
using Microsoft.Extensions.Caching.Hybrid;

namespace AutoVerdikt.Application.Whitelist;

public sealed class WhitelistService(
    IWhitelistRepository repository,
    IFeatureFlagService featureFlags,
    HybridCache cache,
    TimeProvider timeProvider) : IWhitelistService
{
    private static readonly HybridCacheEntryOptions CacheEntryOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromSeconds(60),
        Expiration = TimeSpan.FromSeconds(60),
    };

    private const string CacheKeyPrefix = "whitelist:";

    public async Task<bool> HasAccessAsync(string authId, CancellationToken cancellationToken = default)
    {
        if (!featureFlags.IsWhitelistEnabled())
            return true;

        return await IsWhitelistedAsync(authId, cancellationToken);
    }

    public async Task<bool> IsWhitelistedAsync(string authId, CancellationToken cancellationToken = default)
    {
        var normalized = IdentityNormalizer.NormalizeAuthId(authId);
        var cacheKey = CacheKeyPrefix + normalized;

        return await cache.GetOrCreateAsync(
            cacheKey,
            async ct => await repository.ExistsByAuthIdAsync(normalized, ct),
            CacheEntryOptions,
            cancellationToken: cancellationToken);
    }

    public async Task AddAsync(
        string authId,
        string email,
        Guid? addedBy,
        CancellationToken cancellationToken = default)
    {
        var normalizedAuthId = IdentityNormalizer.NormalizeAuthId(authId);
        var normalizedEmail = IdentityNormalizer.NormalizeEmail(email);
        var cacheKey = CacheKeyPrefix + normalizedAuthId;

        if (await repository.ExistsByAuthIdAsync(normalizedAuthId, cancellationToken))
        {
            await cache.SetAsync(cacheKey, true, CacheEntryOptions, cancellationToken: cancellationToken);
            return;
        }

        var entry = WhitelistEntry.Create(normalizedAuthId, normalizedEmail, addedBy, timeProvider);
        await repository.AddAsync(entry, cancellationToken);
        await cache.SetAsync(cacheKey, true, CacheEntryOptions, cancellationToken: cancellationToken);
    }

    public async Task<bool> RemoveAsync(string authId, CancellationToken cancellationToken = default)
    {
        var normalized = IdentityNormalizer.NormalizeAuthId(authId);
        await cache.RemoveAsync(CacheKeyPrefix + normalized, cancellationToken);
        return await repository.RemoveByAuthIdAsync(normalized, cancellationToken);
    }

    public async Task<PaginatedResult<WhitelistListItem>> ListAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.ListAsync(page, pageSize, cancellationToken);
        var items = result.Items
            .Select(e => new WhitelistListItem(e.AuthId, e.Email, e.CreatedAt, e.AddedBy))
            .ToList();
        return new PaginatedResult<WhitelistListItem>(items, result.Total);
    }
}
