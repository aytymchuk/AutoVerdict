using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Research.Errors;

public sealed class ResearchNotFoundError()
    : DomainError("research_not_found", "Research record not found.", HttpStatusCode.NotFound);
