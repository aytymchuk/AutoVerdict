using FluentResults;
using Mediator;
using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users.Register;

public sealed class UserRegisterCommandHandler(
    IUserRepository repository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider)
    : IRequestHandler<UserRegisterCommand, Result<UserAccount>>
{
    private const string AlreadyRegisteredMessage = "User is already registered.";

    public async ValueTask<Result<UserAccount>> Handle(
        UserRegisterCommand command, CancellationToken cancellationToken)
    {
        if (await repository.ExistsByAuthIdAsync(currentUser.AuthId, cancellationToken))
            return Result.Fail(AlreadyRegisteredMessage);

        var user = UserAccount.Create(currentUser.AuthId, command.Name, command.Email, timeProvider);
        try
        {
            await repository.CreateAsync(user, cancellationToken);
        }
        catch (DuplicateUserException ex)
        {
            return Result.Fail(ex.Message);
        }
        return Result.Ok(user);
    }
}
