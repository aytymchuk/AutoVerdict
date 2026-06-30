using AutoVerdikt.Application.Users.GetCurrent;
using AutoVerdikt.Application.Users.Register;
using AutoVerdikt.Application.Users.UpdateProfile;
using AutoVerdikt.WebApi.Extensions;
using Mediator;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace AutoVerdikt.WebApi.Endpoints.Users;

internal static class UserEndpoints
{
    internal static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(UserEndpointConstants.RegisterRoute,
            async (UserRegistrationDto dto, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new UserRegisterCommand(dto.Name, dto.Email), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();

                var u = result.Value;
                return Results.Created(
                    UserEndpointConstants.GetCurrentRoute,
                    new UserAccountDto(u.Id, u.Name, u.Email, u.RegisteredAt,
                        IsWhitelisted: false,
                        WhitelistStatus: u.WhitelistStatus.ToString().ToLowerInvariant(),
                        Language: null, DefaultCurrency: null));
            })
            .WithName(UserEndpointConstants.RegisterName)
            .WithSummary(UserEndpointConstants.RegisterSummary)
            .WithDescription(UserEndpointConstants.RegisterDescription)
            .Produces<UserAccountDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AddFluentValidationAutoValidation();

        app.MapGet(UserEndpointConstants.GetCurrentRoute,
            async (IMediator mediator, CancellationToken ct) =>
            {
                var user = await mediator.Send(new GetCurrentUserQuery(), ct);
                if (user is null)
                    return Results.NotFound();

                return Results.Ok(new UserAccountDto(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.RegisteredAt,
                    user.IsWhitelisted,
                    user.WhitelistStatus.ToString().ToLowerInvariant(),
                    user.Language,
                    user.DefaultCurrency));
            })
            .WithName(UserEndpointConstants.GetCurrentName)
            .WithSummary(UserEndpointConstants.GetCurrentSummary)
            .WithDescription(UserEndpointConstants.GetCurrentDescription)
            .Produces<UserAccountDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        app.MapPatch(UserEndpointConstants.UpdateProfileRoute,
            async (UpdateUserProfileDto dto, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(
                    new UpdateUserProfileCommand(
                        string.IsNullOrWhiteSpace(dto.Language) ? null : dto.Language,
                        string.IsNullOrWhiteSpace(dto.DefaultCurrency) ? null : dto.DefaultCurrency), ct);
                if (result.IsFailed)
                    return result.ToProblemResult();

                var u = result.Value;
                return Results.Ok(new UserAccountDto(
                    u.Id, u.Name, u.Email, u.RegisteredAt,
                    u.IsWhitelisted,
                    u.WhitelistStatus.ToString().ToLowerInvariant(),
                    u.Language,
                    u.DefaultCurrency));
            })
            .WithName(UserEndpointConstants.UpdateProfileName)
            .WithSummary(UserEndpointConstants.UpdateProfileSummary)
            .WithDescription(UserEndpointConstants.UpdateProfileDescription)
            .Produces<UserAccountDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem()
            .AddFluentValidationAutoValidation();

        return app;
    }
}
