using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users.GetCurrent;

public sealed record CurrentUserDto(
    Guid Id,
    string Name,
    string Email,
    DateTimeOffset RegisteredAt,
    bool IsWhitelisted,
    WhitelistStatus WhitelistStatus);
