using AutoVerdikt.Application.Abstractions;

namespace AutoVerdikt.Application.Research.Delete;

public sealed class DeleteResearchCommandHandler(
    IResearchRepository repository,
    ICurrentUserContext currentUser)
    : IRequestHandler<DeleteResearchCommand, Result>
{
    public async ValueTask<Result> Handle(
        DeleteResearchCommand command,
        CancellationToken cancellationToken) =>
        await repository.DeleteAsync(command.Id, currentUser.AuthId, cancellationToken);
}
