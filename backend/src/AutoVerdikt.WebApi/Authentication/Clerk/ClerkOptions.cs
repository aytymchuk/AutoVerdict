namespace AutoVerdikt.WebApi.Authentication.Clerk;

public class ClerkOptions
{
    public const string SectionName = "Clerk";

    public string Authority { get; set; } = string.Empty;

    // Used for CORS — the frontend origin (e.g. http://localhost:5173)
    public string? AuthorizedParty { get; set; }

    // Additional azp values accepted in JWT validation (e.g. Scalar OAuth client ID)
    public string[] AdditionalAuthorizedParties { get; set; } = [];
}
