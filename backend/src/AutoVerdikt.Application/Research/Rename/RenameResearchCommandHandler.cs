using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Rename;

public sealed class RenameResearchCommandHandler(
    IResearchRepository repository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider)
    : IRequestHandler<RenameResearchCommand, Result<ResearchRecord>>
{
    public async ValueTask<Result<ResearchRecord>> Handle(
        RenameResearchCommand command,
        CancellationToken cancellationToken)
    {
        var getResult = await repository.GetByIdAndAuthIdAsync(command.Id, currentUser.AuthId, cancellationToken);
        if (getResult.IsFailed)
            return getResult.ToResult<ResearchRecord>();

        if (getResult.Value is null)
            return Result.Fail(new ResearchNotFoundError());

        var renamed = getResult.Value.Rename(command.NewName, timeProvider);
        var updateResult = await repository.UpdateAsync(renamed, currentUser.AuthId, cancellationToken);
        if (updateResult.IsFailed)
            return updateResult.ToResult<ResearchRecord>();

        return Result.Ok(renamed);
    }
}
