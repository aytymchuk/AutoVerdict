using FluentResults;

namespace AutoVerdikt.Application.Users.Errors;

public sealed class UserAlreadyRegisteredError() : Error("User is already registered.");
