using AutoVerdikt.Application.Common;
using AutoVerdikt.Domain.Whitelist;

namespace AutoVerdikt.Application.Whitelist;

public interface IWhitelistRepository
{
    Task<bool> ExistsByAuthIdAsync(string authId, CancellationToken cancellationToken = default);
    Task AddAsync(WhitelistEntry entry, CancellationToken cancellationToken = default);
    Task<bool> RemoveByAuthIdAsync(string authId, CancellationToken cancellationToken = default);
    Task<PaginatedResult<WhitelistEntry>> ListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
