namespace AutoVerdikt.WebApi.Authentication.Clerk;

public class ClerkOptions
{
    public const string SectionName = "Clerk";

    public string Authority { get; set; } = string.Empty;
    public string? AuthorizedParty { get; set; }
    // TODO: Move to a dedicated Scalar configuration section
    public string ClientId { get; set; } = string.Empty;
}
