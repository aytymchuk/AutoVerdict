using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create.StartForm;

public sealed record StartFormResearchCommand(CarData Car) : IRequest<Result<ResearchRecord>>;