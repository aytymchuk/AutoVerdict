namespace AutoVerdikt.WebApi.Endpoints.Product;

internal static class ProductEndpointConstants
{
    internal const string AccessRoute = "/product/access";
    internal const string AccessName = "GetProductAccess";
    internal const string AccessSummary = "Verify product access";
    internal const string AccessDescription = "Returns OK when the authenticated user has whitelist access to product features.";
}
