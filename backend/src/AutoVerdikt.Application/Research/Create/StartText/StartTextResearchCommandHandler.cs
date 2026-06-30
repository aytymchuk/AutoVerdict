using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.AI.Extraction;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Application.Research.Create.StartText;

public sealed class StartTextResearchCommandHandler(
    IResearchRepository repository,
    ICurrentUserContext currentUser,
    TimeProvider timeProvider,
    IExtractionService extractionService)
    : StartResearchCommandHandlerBase(repository, currentUser, timeProvider),
      IRequestHandler<StartTextResearchCommand, Result<ResearchRecord>>
{
    public async ValueTask<Result<ResearchRecord>> Handle(
        StartTextResearchCommand command,
        CancellationToken cancellationToken)
    {
        var extraction = await extractionService.ExtractAsync<CarListingFacts>(command.Text, cancellationToken);
        if (!extraction.IsSuccess)
            return Result.Fail<ResearchRecord>(new ExtractionFailedError(
                extraction.ErrorMessage ?? "Failed to extract car listing data from the provided text."));

        var facts = extraction.Value!;
        return await CreateAndPersistAsync(
            InputMethod.Text,
            facts.ToCarData(),
            facts.Description,
            DescriptionSource.AiGeneratedFromText,
            command.Text,
            cancellationToken);
    }
}
