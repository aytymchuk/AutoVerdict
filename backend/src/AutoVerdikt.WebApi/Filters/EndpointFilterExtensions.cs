namespace AutoVerdikt.WebApi.Filters;

internal static class EndpointFilterExtensions
{
    internal static RouteHandlerBuilder RequireWhitelistedAccess(this RouteHandlerBuilder builder) =>
        builder.AddEndpointFilter<WhitelistEndpointFilter>();
}
