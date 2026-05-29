using Mediator;
using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users.GetCurrent;

public sealed class GetCurrentUserQueryHandler(
    IUserRepository repository,
    ICurrentUserContext currentUser)
    : IRequestHandler<GetCurrentUserQuery, UserAccount?>
{
    public async ValueTask<UserAccount?> Handle(
        GetCurrentUserQuery query, CancellationToken cancellationToken)
        => await repository.GetByAuthIdAsync(currentUser.AuthId, cancellationToken);
}
