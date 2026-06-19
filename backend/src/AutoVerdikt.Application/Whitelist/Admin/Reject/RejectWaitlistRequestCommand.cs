using FluentResults;
using Mediator;

namespace AutoVerdikt.Application.Whitelist.Admin.Reject;

public sealed record RejectWaitlistRequestCommand(Guid RequestId) : IRequest<Result>;
