using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Whitelist.Errors;

public sealed class WaitlistRequestNotFoundError()
    : DomainError("waitlist_not_found", "Waitlist request not found.", HttpStatusCode.NotFound);
