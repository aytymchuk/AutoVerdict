namespace AutoVerdikt.Application.Whitelist.Admin.Add;

public sealed record AddToWhitelistCommand(string AuthId, string Email) : IRequest<Result>;
