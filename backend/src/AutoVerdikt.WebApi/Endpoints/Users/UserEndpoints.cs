using AutoVerdikt.Application.Users.Register;
using Mediator;

namespace AutoVerdikt.WebApi.Endpoints.Users;

internal static class UserEndpoints
{
    internal static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(UserEndpointConstants.RegisterRoute,
            async (UserRegistrationDto dto, IMediator mediator, CancellationToken ct) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    return Results.BadRequest("Name is required.");
                if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
                    return Results.BadRequest("A valid email address is required.");

                var result = await mediator.Send(new UserRegisterCommand(dto.Name, dto.Email), ct);
                if (result.IsFailed)
                    return Results.BadRequest(result.Errors.Select(e => e.Message));
                var u = result.Value;
                return Results.Created(
                    $"{UserEndpointConstants.RegisterRoute}/{u.Id}",
                    new UserAccountDto(u.Id, u.Name, u.Email, u.RegisteredAt));
            })
            .WithName(UserEndpointConstants.RegisterName);
        // No AllowAnonymous — global fallback policy (RequireAuthenticatedUser) applies

        return app;
    }
}
