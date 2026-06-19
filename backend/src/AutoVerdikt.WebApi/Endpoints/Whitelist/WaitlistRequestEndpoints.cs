using AutoVerdikt.Application.Whitelist.SubmitWaitlistRequest;
using AutoVerdikt.WebApi.Extensions;
using Mediator;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace AutoVerdikt.WebApi.Endpoints.Whitelist;

internal static class WaitlistRequestEndpoints
{
    internal static IEndpointRouteBuilder MapWaitlistRequestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(WaitlistEndpointConstants.SubmitRoute,
            async (SubmitWaitlistRequestDto dto, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new SubmitWaitlistRequestCommand(dto.About), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();

                return Results.Created(WaitlistEndpointConstants.SubmitRoute, new { status = "pending" });
            })
            .WithName(WaitlistEndpointConstants.SubmitName)
            .WithSummary(WaitlistEndpointConstants.SubmitSummary)
            .WithDescription(WaitlistEndpointConstants.SubmitDescription)
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem()
            .AddFluentValidationAutoValidation();

        return app;
    }
}
