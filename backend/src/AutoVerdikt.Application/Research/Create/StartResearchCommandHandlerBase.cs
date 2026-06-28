using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create;

public abstract class StartResearchCommandHandlerBase(
    IResearchRepository repository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider)
{
    protected async Task<Result<ResearchRecord>> CreateAndPersistAsync(
        InputMethod inputMethod,
        CarData? car,
        string? description,
        DescriptionSource? descriptionSource,
        CancellationToken cancellationToken)
    {
        var record = ResearchRecord.Create(
            currentUser.AuthId,
            inputMethod,
            car,
            description,
            descriptionSource,
            timeProvider);

        var createResult = await repository.CreateAsync(record, cancellationToken);
        if (createResult.IsFailed)
            return createResult.ToResult<ResearchRecord>();

        return Result.Ok(record);
    }
}
