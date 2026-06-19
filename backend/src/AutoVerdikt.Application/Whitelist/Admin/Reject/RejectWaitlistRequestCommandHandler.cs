using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Whitelist.Errors;
using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Whitelist.Admin.Reject;

public sealed class RejectWaitlistRequestCommandHandler(
    IWaitlistRequestRepository repository,
    IUserRepository userRepository,
    TimeProvider timeProvider)
    : IRequestHandler<RejectWaitlistRequestCommand, Result>
{
    public async ValueTask<Result> Handle(
        RejectWaitlistRequestCommand request,
        CancellationToken cancellationToken)
    {
        var waitlistRequest = await repository.GetByIdAsync(request.RequestId, cancellationToken);
        if (waitlistRequest is null)
            return Result.Fail(new WaitlistRequestNotFoundError());

        var now = timeProvider.GetUtcNow();
        var rejected = waitlistRequest.Reject(now);
        await repository.UpdateAsync(rejected, cancellationToken);

        var user = await userRepository.GetByAuthIdAsync(waitlistRequest.AuthId, cancellationToken);
        if (user is not null)
        {
            var declinedUser = user.ChangeWhitelistStatus(WhitelistStatus.Declined);
            await userRepository.UpdateWhitelistStatusAsync(declinedUser, cancellationToken);
        }

        return Result.Ok();
    }
}
