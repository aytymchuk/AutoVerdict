using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Rename;

public sealed record RenameResearchCommand(Guid Id, string NewName)
    : IRequest<Result<ResearchRecord>>;
