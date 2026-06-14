using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Whitelist;
using Mediator;

namespace AutoVerdikt.Application.Users.GetCurrent;

public sealed class GetCurrentUserQueryHandler(
    IUserRepository repository,
    ICurrentUserContext currentUser,
    IWhitelistService whitelistService)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserDto?>
{
    public async ValueTask<CurrentUserDto?> Handle(
        GetCurrentUserQuery query,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByAuthIdAsync(currentUser.AuthId, cancellationToken);
        if (user is null)
            return null;

        var hasAccess = await whitelistService.HasAccessAsync(currentUser.AuthId, cancellationToken);

        return new CurrentUserDto(
            user.Id,
            user.Name,
            user.Email,
            user.RegisteredAt,
            hasAccess,
            user.WhitelistStatus);
    }
}
