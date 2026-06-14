using AutoVerdikt.Application.Common;

namespace AutoVerdikt.Application.Whitelist;

public interface IWhitelistService
{
    Task<bool> IsWhitelistedAsync(string authId, CancellationToken cancellationToken = default);
    Task<bool> HasAccessAsync(string authId, CancellationToken cancellationToken = default);
    Task AddAsync(string authId, string email, Guid? addedBy, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(string authId, CancellationToken cancellationToken = default);
    Task<PaginatedResult<WhitelistListItem>> ListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}

public sealed record WhitelistListItem(
    string AuthId,
    string Email,
    DateTimeOffset CreatedAt,
    Guid? AddedBy);
