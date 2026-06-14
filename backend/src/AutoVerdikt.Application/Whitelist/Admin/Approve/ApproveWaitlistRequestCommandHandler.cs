using AutoVerdikt.Application.Email;
using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Application.Whitelist.Errors;
using AutoVerdikt.Domain.Users;
using AutoVerdikt.Domain.Whitelist;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;

namespace AutoVerdikt.Application.Whitelist.Admin.Approve;

public sealed class ApproveWaitlistRequestCommandHandler(
    IWaitlistRequestRepository repository,
    IUserRepository userRepository,
    ISendGridService sendGridService,
    TimeProvider timeProvider,
    ILogger<ApproveWaitlistRequestCommandHandler> logger)
    : IRequestHandler<ApproveWaitlistRequestCommand, Result>
{
    public async ValueTask<Result> Handle(
        ApproveWaitlistRequestCommand request,
        CancellationToken cancellationToken)
    {
        var waitlistRequest = await repository.GetByIdAsync(request.RequestId, cancellationToken);
        if (waitlistRequest is null)
            return Result.Fail(new WaitlistRequestNotFoundError());

        if (waitlistRequest.Status == WaitlistRequestStatus.Approved)
            return Result.Ok();

        var now = timeProvider.GetUtcNow();
        var approved = waitlistRequest.Approve(now);
        await repository.UpdateAsync(approved, cancellationToken);

        var user = await userRepository.GetByAuthIdAsync(waitlistRequest.AuthId, cancellationToken);
        if (user is not null)
        {
            var approvedUser = user.ChangeWhitelistStatus(WhitelistStatus.Approved);
            await userRepository.UpdateWhitelistStatusAsync(approvedUser, cancellationToken);
        }

        try
        {
            await sendGridService.SendApprovalEmailAsync(
                waitlistRequest.Email,
                waitlistRequest.Locale,
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "approval_email_failed auth_id={AuthId} email={Email}",
                waitlistRequest.AuthId,
                waitlistRequest.Email);
        }

        return Result.Ok();
    }
}
