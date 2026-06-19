namespace AutoVerdikt.Application.Whitelist.Admin.Approve;

public sealed record ApproveWaitlistRequestCommand(Guid RequestId) : IRequest<Result>;
