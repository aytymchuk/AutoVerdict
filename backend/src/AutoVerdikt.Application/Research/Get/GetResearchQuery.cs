using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Get;

public sealed record GetResearchQuery(Guid Id)
    : IRequest<Result<ResearchRecord>>;
