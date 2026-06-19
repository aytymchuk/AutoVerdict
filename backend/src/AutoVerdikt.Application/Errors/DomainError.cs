using System.Net;

namespace AutoVerdikt.Application.Errors;

public abstract class DomainError(string errorCode, string message, HttpStatusCode httpStatusCode)
    : Error(message)
{
    public string ErrorCode { get; } = errorCode;
    public HttpStatusCode HttpStatusCode { get; } = httpStatusCode;
}
