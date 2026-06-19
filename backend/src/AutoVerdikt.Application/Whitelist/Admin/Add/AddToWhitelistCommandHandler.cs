using FluentResults;
using Mediator;

namespace AutoVerdikt.Application.Whitelist.Admin.Add;

public sealed class AddToWhitelistCommandHandler(IWhitelistService whitelistService)
    : IRequestHandler<AddToWhitelistCommand, Result>
{
    public async ValueTask<Result> Handle(AddToWhitelistCommand request, CancellationToken cancellationToken)
    {
        await whitelistService.AddAsync(
            request.AuthId,
            request.Email,
            addedBy: null,
            cancellationToken);
        return Result.Ok();
    }
}
