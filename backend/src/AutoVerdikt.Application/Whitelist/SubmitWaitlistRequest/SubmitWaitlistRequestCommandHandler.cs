using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Identity;
using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Whitelist.Errors;
using AutoVerdikt.Domain.Users;
using AutoVerdikt.Domain.Whitelist;

namespace AutoVerdikt.Application.Whitelist.SubmitWaitlistRequest;

public sealed class SubmitWaitlistRequestCommandHandler(
    IWaitlistRequestRepository repository,
    IUserRepository userRepository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider)
    : IRequestHandler<SubmitWaitlistRequestCommand, Result>
{
    public async ValueTask<Result> Handle(
        SubmitWaitlistRequestCommand request,
        CancellationToken cancellationToken)
    {
        var authId = IdentityNormalizer.NormalizeAuthId(currentUser.AuthId);
        var user = await userRepository.GetByAuthIdAsync(authId, cancellationToken);
        var email = user is not null
            ? IdentityNormalizer.NormalizeEmail(user.Email)
            : IdentityNormalizer.NormalizeEmail(currentUser.Email);

        if (await repository.ExistsByAuthIdAsync(authId, cancellationToken)
            || await repository.ExistsByEmailAsync(email, cancellationToken))
        {
            return Result.Fail(new WaitlistAlreadySubmittedError());
        }

        var waitlistRequest = WaitlistRequest.Create(
            authId,
            email,
            request.About,
            currentUser.Locale,
            timeProvider);

        await repository.CreateAsync(waitlistRequest, cancellationToken);

        if (user is not null)
        {
            var requestedUser = user.ChangeWhitelistStatus(WhitelistStatus.Requested);
            await userRepository.UpdateWhitelistStatusAsync(requestedUser, cancellationToken);
        }

        return Result.Ok();
    }
}
