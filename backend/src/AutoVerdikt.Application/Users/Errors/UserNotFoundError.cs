using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Users.Errors;

public sealed class UserNotFoundError()
    : DomainError("USR-002", "User not found.", HttpStatusCode.NotFound);
