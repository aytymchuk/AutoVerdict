using Mediator;

namespace AutoVerdikt.Application.Whitelist.Admin.GetWaitlistRequests;

public sealed class GetWaitlistRequestsQueryHandler(IWaitlistRequestRepository repository)
    : IRequestHandler<GetWaitlistRequestsQuery, IReadOnlyList<WaitlistRequestListItem>>
{
    public async ValueTask<IReadOnlyList<WaitlistRequestListItem>> Handle(
        GetWaitlistRequestsQuery query,
        CancellationToken cancellationToken)
    {
        var requests = await repository.ListByStatusAsync(query.Status, cancellationToken);
        return requests
            .Select(r => new WaitlistRequestListItem(
                r.Id,
                r.AuthId,
                r.Email,
                r.About,
                r.Status,
                r.Locale,
                r.CreatedAt,
                r.UpdatedAt))
            .ToList();
    }
}
