using FluentResults;
using Mediator;

namespace AutoVerdikt.Application.Whitelist.SubmitWaitlistRequest;

public sealed record SubmitWaitlistRequestCommand(string? About) : IRequest<Result>;
