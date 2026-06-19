using AutoVerdikt.Domain.Whitelist;

namespace AutoVerdikt.Application.Whitelist;

public interface IWaitlistRequestRepository
{
    Task<bool> ExistsByAuthIdAsync(string authId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task CreateAsync(WaitlistRequest request, CancellationToken cancellationToken = default);
    Task<WaitlistRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(WaitlistRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WaitlistRequest>> ListByStatusAsync(
        WaitlistRequestStatus? status,
        CancellationToken cancellationToken = default);
}
