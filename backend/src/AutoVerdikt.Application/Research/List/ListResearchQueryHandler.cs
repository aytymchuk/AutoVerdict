using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Common;

namespace AutoVerdikt.Application.Research.List;

public sealed class ListResearchQueryHandler(
    IResearchRepository repository,
    ICurrentUserContext currentUser)
    : IRequestHandler<ListResearchQuery, Result<PaginatedResult<Domain.Research.ResearchRecord>>>
{
    public async ValueTask<Result<PaginatedResult<Domain.Research.ResearchRecord>>> Handle(
        ListResearchQuery query,
        CancellationToken cancellationToken) =>
        await repository.ListByAuthIdAsync(
            currentUser.AuthId,
            query.Page,
            query.PageSize,
            cancellationToken);
}
