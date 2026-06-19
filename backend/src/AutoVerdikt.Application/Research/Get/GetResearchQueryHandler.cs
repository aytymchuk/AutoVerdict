using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Get;

public sealed class GetResearchQueryHandler(
    IResearchRepository repository,
    ICurrentUserContext currentUser)
    : IRequestHandler<GetResearchQuery, Result<ResearchRecord>>
{
    public async ValueTask<Result<ResearchRecord>> Handle(
        GetResearchQuery query,
        CancellationToken cancellationToken)
    {
        var getResult = await repository.GetByIdAndAuthIdAsync(query.Id, currentUser.AuthId, cancellationToken);
        if (getResult.IsFailed)
            return getResult.ToResult<ResearchRecord>();

        if (getResult.Value is null)
            return Result.Fail(new ResearchNotFoundError());

        return Result.Ok(getResult.Value);
    }
}
