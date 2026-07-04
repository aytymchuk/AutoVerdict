using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Store.Research;

internal static class ResearchDocumentMapper
{
    public static ResearchDocument ToDocument(ResearchRecord record) =>
        new()
        {
            Id = record.Id,
            AuthId = record.AuthId,
            Name = record.Name,
            Status = ToStatusString(record.Status),
            RiskLevel = ToRiskLevelString(record.RiskLevel),
            InputMethod = ToInputMethodString(record.InputMethod),
            Car = record.Car is null ? null : CarDataMapper.ToDocument(record.Car),
            Description = record.Description,
            DescriptionSource = ToDescriptionSourceString(record.DescriptionSource),
            InitialPrompt = record.InitialPrompt,
            CreditsSpent = record.CreditsSpent,
            IsNameManual = record.IsNameManual,
            LastAnalyzedAt = record.LastAnalyzedAt?.UtcDateTime,
            RetentionPolicy = record.RetentionPolicy,
            CreatedAt = record.CreatedAt.UtcDateTime,
            UpdatedAt = record.UpdatedAt.UtcDateTime
        };

    public static ResearchRecord ToDomain(
        ResearchDocument document,
        IReadOnlyList<Note> notes,
        IReadOnlyList<Detail> details,
        IReadOnlyList<AttachedFile> files,
        IReadOnlyList<Question> questions,
        IReadOnlyList<Badge> badges) =>
        new()
        {
            Id = document.Id,
            AuthId = document.AuthId,
            Name = document.Name,
            Status = ParseStatus(document.Status),
            RiskLevel = ParseRiskLevel(document.RiskLevel),
            InputMethod = ParseInputMethod(document.InputMethod),
            Car = document.Car is null ? null : CarDataMapper.ToDomain(document.Car),
            Description = document.Description,
            DescriptionSource = ParseDescriptionSource(document.DescriptionSource),
            InitialPrompt = document.InitialPrompt,
            CreditsSpent = document.CreditsSpent,
            IsNameManual = document.IsNameManual,
            LastAnalyzedAt = document.LastAnalyzedAt is null
                ? null
                : new DateTimeOffset(document.LastAnalyzedAt.Value, TimeSpan.Zero),
            RetentionPolicy = document.RetentionPolicy,
            CreatedAt = new DateTimeOffset(document.CreatedAt, TimeSpan.Zero),
            UpdatedAt = new DateTimeOffset(document.UpdatedAt, TimeSpan.Zero),
            Notes = notes,
            Details = details,
            Files = files,
            Questions = questions,
            Badges = badges
        };

    public static ResearchRecord ToDomainListItem(ResearchDocument document) =>
        ToDomain(document, [], [], [], [], []);

    private static string ToInputMethodString(InputMethod inputMethod) => inputMethod switch
    {
        InputMethod.Form => "form",
        InputMethod.Text => "text",
        _ => throw new ArgumentOutOfRangeException(nameof(inputMethod), inputMethod, null)
    };

    private static InputMethod ParseInputMethod(string value) => value switch
    {
        "form" => InputMethod.Form,
        "text" or "paste" => InputMethod.Text,
        _ => InputMethod.Form
    };

    private static string ToStatusString(ResearchStatus status) => status switch
    {
        ResearchStatus.Draft => "draft",
        ResearchStatus.Pending => "pending",
        ResearchStatus.Analyzing => "analyzing",
        ResearchStatus.Analyzed => "analyzed",
        ResearchStatus.Failed => "failed",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static ResearchStatus ParseStatus(string? value) => value switch
    {
        null or "draft" => ResearchStatus.Draft,
        "pending" => ResearchStatus.Pending,
        "analyzing" => ResearchStatus.Analyzing,
        "analyzed" => ResearchStatus.Analyzed,
        "failed" => ResearchStatus.Failed,
        _ => ResearchStatus.Draft
    };

    private static string? ToRiskLevelString(RiskLevel? riskLevel) => riskLevel switch
    {
        null => null,
        RiskLevel.Low => "low",
        RiskLevel.Medium => "medium",
        RiskLevel.High => "high",
        _ => throw new ArgumentOutOfRangeException(nameof(riskLevel), riskLevel, null)
    };

    private static RiskLevel? ParseRiskLevel(string? value) => value switch
    {
        null => null,
        "low" => RiskLevel.Low,
        "medium" => RiskLevel.Medium,
        "high" => RiskLevel.High,
        _ => null
    };

    private static string? ToDescriptionSourceString(DescriptionSource? descriptionSource) =>
        descriptionSource switch
        {
            null => null,
            DescriptionSource.AiGeneratedFromText => "aiGeneratedFromText",
            DescriptionSource.AiGeneratedFromFacts => "aiGeneratedFromFacts",
            _ => throw new ArgumentOutOfRangeException(nameof(descriptionSource), descriptionSource, null)
        };

    private static DescriptionSource? ParseDescriptionSource(string? value) => value switch
    {
        null => null,
        "aiGeneratedFromText" => DescriptionSource.AiGeneratedFromText,
        "aiGeneratedFromFacts" => DescriptionSource.AiGeneratedFromFacts,
        _ => null
    };
}
