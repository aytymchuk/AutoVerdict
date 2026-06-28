namespace AutoVerdikt.Application.Whitelist.Admin.Remove;

public sealed record RemoveFromWhitelistCommand(string AuthId) : IRequest<Result>;
