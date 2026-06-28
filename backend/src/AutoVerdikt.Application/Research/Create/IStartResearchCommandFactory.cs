using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create;

public interface IStartResearchCommandFactory
{
    Result<IRequest<Result<ResearchRecord>>> Create(CreateResearchCommand command);
}
