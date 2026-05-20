using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users;

public interface IUserRepository
{
    Task<bool> ExistsByClerkIdAsync(string clerkId, CancellationToken cancellationToken = default);
    Task CreateAsync(UserAccount user, CancellationToken cancellationToken = default);
}
