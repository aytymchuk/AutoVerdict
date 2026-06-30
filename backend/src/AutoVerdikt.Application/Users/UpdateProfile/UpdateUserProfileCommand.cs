using FluentResults;
using Mediator;
using AutoVerdikt.Application.Users.GetCurrent;

namespace AutoVerdikt.Application.Users.UpdateProfile;

public sealed record UpdateUserProfileCommand(string? Language, string? DefaultCurrency)
    : IRequest<Result<CurrentUserDto>>;
