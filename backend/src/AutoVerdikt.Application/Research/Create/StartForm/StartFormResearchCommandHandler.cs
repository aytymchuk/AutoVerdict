using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create.StartForm;

public sealed class StartFormResearchCommandHandler(
    IResearchRepository repository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider)
    : StartResearchCommandHandlerBase(repository, currentUser, timeProvider),
      IRequestHandler<StartFormResearchCommand, Result<ResearchRecord>>
{
    public async ValueTask<Result<ResearchRecord>> Handle(
        StartFormResearchCommand command,
        CancellationToken cancellationToken) =>
        await CreateAndPersistAsync(
            InputMethod.Form,
            command.Car,
            description: null,
            descriptionSource: null,
            cancellationToken);
}
