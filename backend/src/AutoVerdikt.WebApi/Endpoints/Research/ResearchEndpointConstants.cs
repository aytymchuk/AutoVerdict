namespace AutoVerdikt.WebApi.Endpoints.Research;

internal static class ResearchEndpointConstants
{
    internal const string BaseRoute = "/research";
    internal const string CreateRoute = BaseRoute;
    internal const string ListRoute = BaseRoute;
    internal const string GetRoute = $"{BaseRoute}/{{id:guid}}";
    internal const string RenameRoute = $"{BaseRoute}/{{id:guid}}/name";
    internal const string DeleteRoute = $"{BaseRoute}/{{id:guid}}";

    internal const string CreateName = "CreateResearch";
    internal const string CreateSummary = "Create a research record";
    internal const string CreateDescription =
        "Creates a new vehicle research record for the authenticated user using structured form data.";

    internal const string ListName = "ListResearch";
    internal const string ListSummary = "List research records";
    internal const string ListDescription =
        "Returns the authenticated user's research records sorted by most recently updated.";

    internal const string GetName = "GetResearch";
    internal const string GetSummary = "Get a research record";
    internal const string GetDescription =
        "Returns full detail for one research record owned by the authenticated user.";

    internal const string RenameName = "RenameResearch";
    internal const string RenameSummary = "Rename a research record";
    internal const string RenameDescription =
        "Updates the display name of a research record owned by the authenticated user.";

    internal const string DeleteName = "DeleteResearch";
    internal const string DeleteSummary = "Delete a research record";
    internal const string DeleteDescription =
        "Permanently removes a research record owned by the authenticated user.";
}
