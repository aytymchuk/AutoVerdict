namespace AutoVerdikt.WebApi.OpenApi;

internal static class ClerkScopes
{
    // Must match scopes enabled on the Clerk OAuth application (dashboard).
    internal static readonly IReadOnlyDictionary<string, string> Definitions =
        new Dictionary<string, string>
        {
            ["profile"] = "User profile",
            ["email"] = "Email address",
            ["offline_access"] = "Refresh token"
        };

    internal static string[] All => [.. Definitions.Keys];
}
