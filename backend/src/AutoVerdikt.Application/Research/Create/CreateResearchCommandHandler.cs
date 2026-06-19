using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create;

public sealed class CreateResearchCommandHandler(
    IStartResearchCommandFactory factory,
    IMediator mediator)
    : IRequestHandler<CreateResearchCommand, Result<ResearchRecord>>
{
    public async ValueTask<Result<ResearchRecord>> Handle(
        CreateResearchCommand command,
        CancellationToken cancellationToken)
    {
        var startCommandResult = factory.Create(command);
        if (startCommandResult.IsFailed)
            return Result.Fail<ResearchRecord>(startCommandResult.Errors);

        return await mediator.Send(startCommandResult.Value, cancellationToken);
    }
}
