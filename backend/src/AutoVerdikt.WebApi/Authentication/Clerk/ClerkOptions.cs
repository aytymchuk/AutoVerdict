namespace AutoVerdikt.WebApi.Authentication.Clerk;

public class ClerkOptions
{
    public const string SectionName = "Clerk";

    public string Authority { get; set; } = string.Empty;
    public string? AuthorizedParty { get; set; }
}
