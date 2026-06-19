using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create;

public sealed record CreateResearchCommand(InputMethod InputMethod, CarData? Car)
    : IRequest<Result<ResearchRecord>>;
