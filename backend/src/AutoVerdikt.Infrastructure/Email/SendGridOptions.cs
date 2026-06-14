namespace AutoVerdikt.Infrastructure.Email;

public sealed class SendGridOptions
{
    public const string SectionName = "SendGrid";
    public string ApiKey { get; init; } = string.Empty;
    public string FromEmail { get; init; } = string.Empty;
    public string SenderName { get; init; } = "AutoCheck AI";
}
