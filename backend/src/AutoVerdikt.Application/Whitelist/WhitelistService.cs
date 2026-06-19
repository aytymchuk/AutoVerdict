using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Application.Identity;
using AutoVerdikt.Domain.Whitelist;
using Microsoft.Extensions.Caching.Memory;

namespace AutoVerdikt.Application.Whitelist;

public sealed class WhitelistService(
    IWhitelistRepository repository,
    IFeatureFlagService featureFlags,
    IMemoryCache cache,
    TimeProvider timeProvider) : IWhitelistService
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);
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

        if (cache.TryGetValue(cacheKey, out bool cached))
            return cached;

        var exists = await repository.ExistsByAuthIdAsync(normalized, cancellationToken);
        cache.Set(cacheKey, exists, CacheTtl);
        return exists;
    }

    public async Task AddAsync(
        string authId,
        string email,
        Guid? addedBy,
        CancellationToken cancellationToken = default)
    {
        var normalizedAuthId = IdentityNormalizer.NormalizeAuthId(authId);
        var normalizedEmail = IdentityNormalizer.NormalizeEmail(email);

        if (await repository.ExistsByAuthIdAsync(normalizedAuthId, cancellationToken))
        {
            cache.Set(CacheKeyPrefix + normalizedAuthId, true, CacheTtl);
            return;
        }

        var entry = WhitelistEntry.Create(normalizedAuthId, normalizedEmail, addedBy, timeProvider);
        await repository.AddAsync(entry, cancellationToken);
        cache.Set(CacheKeyPrefix + normalizedAuthId, true, CacheTtl);
    }

    public async Task<bool> RemoveAsync(string authId, CancellationToken cancellationToken = default)
    {
        var normalized = IdentityNormalizer.NormalizeAuthId(authId);
        cache.Remove(CacheKeyPrefix + normalized);
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
