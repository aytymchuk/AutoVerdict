using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Whitelist.Errors;

public sealed class WhitelistCheckUnavailableError()
    : DomainError("whitelist_unavailable", "Whitelist check is temporarily unavailable.", HttpStatusCode.ServiceUnavailable);
