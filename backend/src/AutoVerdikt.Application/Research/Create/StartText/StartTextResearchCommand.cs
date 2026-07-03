using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create.StartText;

public sealed record StartTextResearchCommand(string Text) : IRequest<Result<ResearchRecord>>;
