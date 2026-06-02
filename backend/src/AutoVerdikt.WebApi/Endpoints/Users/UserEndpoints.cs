using AutoVerdikt.Application.Users.GetCurrent;
using AutoVerdikt.Application.Users.Register;
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
                    $"{UserEndpointConstants.RegisterRoute}/{u.Id}",
                    new UserAccountDto(u.Id, u.Name, u.Email, u.RegisteredAt));
            })
            .WithName(UserEndpointConstants.RegisterName)
            .AddFluentValidationAutoValidation();
        // No AllowAnonymous — global fallback policy (RequireAuthenticatedUser) applies

        app.MapGet(UserEndpointConstants.GetCurrentRoute,
            async (IMediator mediator, CancellationToken ct) =>
            {
                var user = await mediator.Send(new GetCurrentUserQuery(), ct);
                if (user is null)
                    return Results.NotFound();

                return Results.Ok(new UserAccountDto(user.Id, user.Name, user.Email, user.RegisteredAt));
            })
            .WithName(UserEndpointConstants.GetCurrentName);
        // No AllowAnonymous — global fallback policy (RequireAuthenticatedUser) applies

        return app;
    }
}
