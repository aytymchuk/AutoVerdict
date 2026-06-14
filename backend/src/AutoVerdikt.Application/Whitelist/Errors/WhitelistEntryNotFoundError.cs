using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Whitelist.Errors;

public sealed class WhitelistEntryNotFoundError()
    : DomainError("whitelist_not_found", "Whitelist entry not found.", HttpStatusCode.NotFound);
