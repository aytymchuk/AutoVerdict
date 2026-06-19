namespace AutoVerdikt.Application.Research.Delete;

public sealed record DeleteResearchCommand(Guid Id) : IRequest<Result>;
