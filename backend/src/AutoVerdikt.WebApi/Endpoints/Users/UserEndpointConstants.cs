namespace AutoVerdikt.WebApi.Endpoints.Users;

internal static class UserEndpointConstants
{
    internal const string RegisterRoute = "/users/register";
    internal const string RegisterName = "RegisterUser";
    internal const string RegisterSummary = "Register the authenticated user";
    internal const string RegisterDescription =
        "Creates an account for the currently authenticated Clerk user. " +
        "Returns 409 if the user is already registered and 400 if the request body is invalid.";

    internal const string GetCurrentRoute = "/users/me";
    internal const string GetCurrentName = "GetCurrentUser";
    internal const string GetCurrentSummary = "Get the current user account";
    internal const string GetCurrentDescription =
        "Returns the account of the currently authenticated Clerk user, " +
        "or 404 if no account has been registered yet.";

    internal const string UpdateProfileRoute = "/users/me/profile";
    internal const string UpdateProfileName = "UpdateUserProfile";
    internal const string UpdateProfileSummary = "Update the current user's profile settings";
    internal const string UpdateProfileDescription =
        "Updates the language and default currency preferences for the currently authenticated user.";
}
