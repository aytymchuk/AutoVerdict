using AutoVerdikt.Application.Research.Create.StartForm;
using AutoVerdikt.Application.Research.Create.StartText;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create;

public sealed class StartResearchCommandFactory : IStartResearchCommandFactory
{
    public Result<IRequest<Result<ResearchRecord>>> Create(CreateResearchCommand command) =>
        command.InputMethod switch
        {
            InputMethod.Form => Result.Ok<IRequest<Result<ResearchRecord>>>(new StartFormResearchCommand(command.Car!)),
            InputMethod.Text => Result.Ok<IRequest<Result<ResearchRecord>>>(new StartTextResearchCommand()),
            _ => Result.Fail<IRequest<Result<ResearchRecord>>>(new InputMethodNotSupportedError())
        };
}
