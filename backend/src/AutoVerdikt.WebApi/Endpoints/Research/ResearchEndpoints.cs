using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.Research.Create;
using AutoVerdikt.Application.Research.Delete;
using AutoVerdikt.Application.Research.Get;
using AutoVerdikt.Application.Research.List;
using AutoVerdikt.Application.Research.Rename;
using AutoVerdikt.WebApi.Extensions;
using Mediator;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace AutoVerdikt.WebApi.Endpoints.Research;

internal static class ResearchEndpoints
{
    internal static IEndpointRouteBuilder MapResearchEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ResearchEndpointConstants.CreateRoute,
            async (CreateResearchDto dto, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(
                    new CreateResearchCommand(dto.InputMethod.ToInputMethod(), dto.Car?.ToDomain(), dto.Text),
                    ct);

                if (result.IsFailed)
                    return result.ToProblemResult();

                return Results.Created(
                    $"{ResearchEndpointConstants.BaseRoute}/{result.Value.Id}",
                    result.Value.ToDetailDto());
            })
            .WithName(ResearchEndpointConstants.CreateName)
            .WithSummary(ResearchEndpointConstants.CreateSummary)
            .WithDescription(ResearchEndpointConstants.CreateDescription)
            .Produces<ResearchDetailDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AddFluentValidationAutoValidation();

        app.MapGet(ResearchEndpointConstants.ListRoute,
            async (IMediator mediator, CancellationToken ct, int page = 1, int pageSize = 20) =>
            {
                var result = await mediator.Send(new ListResearchQuery(page, pageSize), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();

                var items = result.Value.Items.Select(r => r.ToListItemDto()).ToList();
                return Results.Ok(new PaginatedResult<ResearchListItemDto>(items, result.Value.Total));
            })
            .WithName(ResearchEndpointConstants.ListName)
            .WithSummary(ResearchEndpointConstants.ListSummary)
            .WithDescription(ResearchEndpointConstants.ListDescription)
            .Produces<PaginatedResult<ResearchListItemDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapGet(ResearchEndpointConstants.GetRoute,
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetResearchQuery(id), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();

                return Results.Ok(result.Value.ToDetailDto());
            })
            .WithName(ResearchEndpointConstants.GetName)
            .WithSummary(ResearchEndpointConstants.GetSummary)
            .WithDescription(ResearchEndpointConstants.GetDescription)
            .Produces<ResearchDetailDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPatch(ResearchEndpointConstants.RenameRoute,
            async (Guid id, RenameResearchDto dto, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new RenameResearchCommand(id, dto.NewName), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();

                return Results.Ok(result.Value.ToDetailDto());
            })
            .WithName(ResearchEndpointConstants.RenameName)
            .WithSummary(ResearchEndpointConstants.RenameSummary)
            .WithDescription(ResearchEndpointConstants.RenameDescription)
            .Produces<ResearchDetailDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AddFluentValidationAutoValidation();

        app.MapDelete(ResearchEndpointConstants.DeleteRoute,
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteResearchCommand(id), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();

                return Results.NoContent();
            })
            .WithName(ResearchEndpointConstants.DeleteName)
            .WithSummary(ResearchEndpointConstants.DeleteSummary)
            .WithDescription(ResearchEndpointConstants.DeleteDescription)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
