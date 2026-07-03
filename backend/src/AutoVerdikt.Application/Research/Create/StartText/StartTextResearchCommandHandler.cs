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

        if (!extraction.IsIntentMatch)
            return Result.Fail<ResearchRecord>(new ExtractionIntentMismatchError(
                extraction.Reasoning ?? "The provided text does not appear to describe a car listing."));

        var facts = extraction.Value;
        if (facts is null)
            return Result.Fail<ResearchRecord>(new ExtractionFailedError("No car listing data could be extracted."));

        var carData = facts.ToCarData();
        if (!carData.HasMinimumRequiredFields())
            return Result.Fail<ResearchRecord>(new ExtractionFailedError(
                "Could not extract enough car details (make, model, year, mileage, and price) from the provided text."));

        return await CreateAndPersistAsync(
            InputMethod.Text,
            carData,
            facts.Description,
            DescriptionSource.AiGeneratedFromText,
            command.Text,
            cancellationToken);
    }
}
