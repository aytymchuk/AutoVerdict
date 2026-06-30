using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Research.Errors;

public sealed class ExtractionFailedError(string message)
    : DomainError("extraction_failed", message, HttpStatusCode.UnprocessableEntity);
