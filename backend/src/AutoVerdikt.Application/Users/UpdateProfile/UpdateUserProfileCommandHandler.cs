using FluentResults;
using Mediator;
using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Users.Errors;
using AutoVerdikt.Application.Users.GetCurrent;
using AutoVerdikt.Application.Whitelist;

namespace AutoVerdikt.Application.Users.UpdateProfile;

public sealed class UpdateUserProfileCommandHandler(
    IUserRepository repository,
    ICurrentUserContext currentUser,
    IWhitelistService whitelistService)
    : IRequestHandler<UpdateUserProfileCommand, Result<CurrentUserDto>>
{
    public async ValueTask<Result<CurrentUserDto>> Handle(
        UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await repository.GetByAuthIdAsync(currentUser.AuthId, cancellationToken);
        if (user is null)
            return Result.Fail(new UserNotFoundError());

        var updated = user.UpdateProfile(command.Language, command.DefaultCurrency);
        await repository.UpdateProfileAsync(updated, cancellationToken);

        var hasAccess = await whitelistService.HasAccessAsync(currentUser.AuthId, cancellationToken);
        return Result.Ok(new CurrentUserDto(
            updated.Id,
            updated.Name,
            updated.Email,
            updated.RegisteredAt,
            hasAccess,
            updated.WhitelistStatus,
            updated.Language,
            updated.DefaultCurrency));
    }
}
