using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Application.Whitelist.Admin.Add;
using AutoVerdikt.Application.Whitelist.Admin.Approve;
using AutoVerdikt.Application.Whitelist.Admin.GetWaitlistRequests;
using AutoVerdikt.Application.Whitelist.Admin.GetWhitelist;
using AutoVerdikt.Application.Whitelist.Admin.Reject;
using AutoVerdikt.Application.Whitelist.Admin.Remove;
using AutoVerdikt.Domain.Whitelist;
using AutoVerdikt.WebApi.Authorization;
using AutoVerdikt.WebApi.Extensions;
using Mediator;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace AutoVerdikt.WebApi.Endpoints.Admin;

internal static class WhitelistAdminEndpoints
{
    internal static IEndpointRouteBuilder MapWhitelistAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty)
            .RequireAuthorization(AdminAuthorizationExtensions.AdminPolicyName);

        group.MapGet(WhitelistAdminEndpointConstants.WhitelistRoute,
            async (int page, int pageSize, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetWhitelistQuery(page, pageSize), ct);
                return Results.Ok(result);
            })
            .Produces<PaginatedResult<WhitelistListItem>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapPost(WhitelistAdminEndpointConstants.WhitelistRoute,
            async (AddWhitelistEntryDto dto, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new AddToWhitelistCommand(dto.AuthId, dto.Email), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();
                return Results.Ok();
            })
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapDelete($"{WhitelistAdminEndpointConstants.WhitelistRoute}/{{authId}}",
            async (string authId, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new RemoveFromWhitelistCommand(authId), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapGet(WhitelistAdminEndpointConstants.WaitlistRequestsRoute,
            async (string? status, IMediator mediator, CancellationToken ct) =>
            {
                WaitlistRequestStatus? parsed = status switch
                {
                    "pending" => WaitlistRequestStatus.Pending,
                    "approved" => WaitlistRequestStatus.Approved,
                    "rejected" => WaitlistRequestStatus.Rejected,
                    _ => null
                };
                var result = await mediator.Send(new GetWaitlistRequestsQuery(parsed), ct);
                return Results.Ok(result);
            })
            .Produces<IReadOnlyList<WaitlistRequestListItem>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapPost($"{WhitelistAdminEndpointConstants.WaitlistRequestsRoute}/{{id:guid}}/approve",
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ApproveWaitlistRequestCommand(id), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapPost($"{WhitelistAdminEndpointConstants.WaitlistRequestsRoute}/{{id:guid}}/reject",
            async (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new RejectWaitlistRequestCommand(id), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }
}
