using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Research.Errors;

public sealed class ExtractionIntentMismatchError(string message)
    : DomainError("extraction_intent_mismatch", message, HttpStatusCode.UnprocessableEntity);
