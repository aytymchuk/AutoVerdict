using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Application.Whitelist.Errors;

namespace AutoVerdikt.WebApi.Filters;

internal sealed class WhitelistEndpointFilter(
    IFeatureFlagService featureFlags,
    IWhitelistService whitelistService,
    ILogger<WhitelistEndpointFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        if (!featureFlags.IsWhitelistEnabled())
            return await next(context);

        var httpContext = context.HttpContext;
        if (httpContext.User.Identity?.IsAuthenticated != true)
            return Results.Unauthorized();

        var authId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(authId))
            return Results.Unauthorized();

        try
        {
            var whitelisted = await whitelistService.IsWhitelistedAsync(authId, httpContext.RequestAborted);
            if (!whitelisted)
                return Results.Json(
                    new { errorCode = "whitelist_required", message = "Access is limited to whitelisted users." },
                    statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Whitelist check failed for auth_id {AuthId}", authId);
            return Results.Json(
                new WhitelistCheckUnavailableError().Message,
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        return await next(context);
    }
}
