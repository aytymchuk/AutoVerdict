using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Application.Whitelist.Errors;
using FluentResults;
using Mediator;

namespace AutoVerdikt.Application.Whitelist.Admin.Remove;

public sealed class RemoveFromWhitelistCommandHandler(IWhitelistService whitelistService)
    : IRequestHandler<RemoveFromWhitelistCommand, Result>
{
    public async ValueTask<Result> Handle(RemoveFromWhitelistCommand request, CancellationToken cancellationToken)
    {
        var removed = await whitelistService.RemoveAsync(request.AuthId, cancellationToken);
        if (!removed)
            return Result.Fail(new WhitelistEntryNotFoundError());

        return Result.Ok();
    }
}
