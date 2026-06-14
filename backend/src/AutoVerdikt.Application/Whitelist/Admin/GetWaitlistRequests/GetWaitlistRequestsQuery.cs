using AutoVerdikt.Domain.Whitelist;
using Mediator;

namespace AutoVerdikt.Application.Whitelist.Admin.GetWaitlistRequests;

public sealed record GetWaitlistRequestsQuery(WaitlistRequestStatus? Status)
    : IRequest<IReadOnlyList<WaitlistRequestListItem>>;

public sealed record WaitlistRequestListItem(
    Guid Id,
    string AuthId,
    string Email,
    string? About,
    WaitlistRequestStatus Status,
    string Locale,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
