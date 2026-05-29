using Mediator;
using AutoVerdikt.Domain.Users;

namespace AutoVerdikt.Application.Users.GetCurrent;

public sealed record GetCurrentUserQuery : IRequest<UserAccount?>;
