using System.Net;
using AutoVerdikt.Application.Errors;

namespace AutoVerdikt.Application.Users.Errors;

public sealed class UserAlreadyRegisteredError()
    : DomainError("USR-001", "User is already registered.", HttpStatusCode.Conflict);
