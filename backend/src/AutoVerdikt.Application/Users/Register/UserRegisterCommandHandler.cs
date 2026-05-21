using FluentResults;
using Mediator;
using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Users.Errors;
using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users.Register;

public sealed class UserRegisterCommandHandler(
    IUserRepository repository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider)
    : IRequestHandler<UserRegisterCommand, Result<UserAccount>>
{
    public async ValueTask<Result<UserAccount>> Handle(
        UserRegisterCommand command, CancellationToken cancellationToken)
    {
        if (await repository.ExistsByAuthIdAsync(currentUser.AuthId, cancellationToken))
            return Result.Fail(new UserAlreadyRegisteredError());

        var user = UserAccount.Create(currentUser.AuthId, command.Name, command.Email, timeProvider);
        var createResult = await repository.CreateAsync(user, cancellationToken);
        if (createResult.IsFailed)
            return createResult;

        return Result.Ok(user);
    }
}
