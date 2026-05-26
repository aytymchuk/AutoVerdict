using AutoVerdikt.Application.Users.Errors;
using AutoVerdikt.Application.Users.Register;
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
                {
                    var alreadyRegistered = result.Errors.OfType<UserAlreadyRegisteredError>().FirstOrDefault();
                    if (alreadyRegistered is not null)
                        return Results.Conflict(new { error = alreadyRegistered.Message });

                    return Results.BadRequest(result.Errors.Select(e => e.Message));
                }
                var u = result.Value;
                return Results.Created(
                    $"{UserEndpointConstants.RegisterRoute}/{u.Id}",
                    new UserAccountDto(u.Id, u.Name, u.Email, u.RegisteredAt));
            })
            .WithName(UserEndpointConstants.RegisterName)
            .AddFluentValidationAutoValidation();
        // No AllowAnonymous — global fallback policy (RequireAuthenticatedUser) applies

        return app;
    }
}
