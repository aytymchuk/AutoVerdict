using FluentResults;
using Mediator;
using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users.Register;

public sealed record UserRegisterCommand(string Name, string Email)
    : IRequest<Result<UserAccount>>;
