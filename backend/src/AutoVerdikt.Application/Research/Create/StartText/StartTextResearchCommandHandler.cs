using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create.StartText;

public sealed class StartTextResearchCommandHandler(
    IResearchRepository repository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider)
    : StartResearchCommandHandlerBase(repository, currentUser, timeProvider),
      IRequestHandler<StartTextResearchCommand, Result<ResearchRecord>>
{
    public ValueTask<Result<ResearchRecord>> Handle(
        StartTextResearchCommand command,
        CancellationToken cancellationToken) =>
        ValueTask.FromResult(Result.Fail<ResearchRecord>(new InputMethodNotSupportedError()));
}
