using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Research.Errors;

public sealed class InputMethodNotSupportedError()
    : DomainError("input_method_not_supported", "This input method is not supported yet.", HttpStatusCode.UnprocessableEntity);
