using FluentResults;
using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users;

public interface IUserRepository
{
    Task<bool> ExistsByAuthIdAsync(string authId, CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(UserAccount user, CancellationToken cancellationToken = default);
}
