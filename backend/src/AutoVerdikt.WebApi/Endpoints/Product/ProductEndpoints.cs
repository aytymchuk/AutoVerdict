using AutoVerdikt.WebApi.Filters;

namespace AutoVerdikt.WebApi.Endpoints.Product;

internal static class ProductEndpoints
{
    internal static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ProductEndpointConstants.AccessRoute, () => Results.Ok(new { access = true }))
            .WithName(ProductEndpointConstants.AccessName)
            .WithSummary(ProductEndpointConstants.AccessSummary)
            .WithDescription(ProductEndpointConstants.AccessDescription)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status503ServiceUnavailable)
            .RequireWhitelistedAccess();

        return app;
    }
}
