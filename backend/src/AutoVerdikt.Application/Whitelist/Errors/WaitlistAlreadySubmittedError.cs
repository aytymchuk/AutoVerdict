using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Whitelist.Errors;

public sealed class WaitlistAlreadySubmittedError()
    : DomainError("already_submitted", "Your request is already in the queue.", HttpStatusCode.Conflict);
