namespace AutoVerdikt.Domain.Research;

public record ResearchRecord
{
    public required Guid Id { get; init; }
    public required string AuthId { get; init; }
    public string? Name { get; init; }
    public ResearchStatus Status { get; init; } = ResearchStatus.Draft;
    public RiskLevel? RiskLevel { get; init; }
    public required InputMethod InputMethod { get; init; }
    public CarData? Car { get; init; }
    public string? Description { get; init; }
    public DescriptionSource? DescriptionSource { get; init; }
    public string? InitialPrompt { get; init; }
    public int CreditsSpent { get; init; }
    public bool IsNameManual { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }

    public static ResearchRecord Create(
        string authId,
        InputMethod inputMethod,
        CarData? car,
        string? description,
        DescriptionSource? descriptionSource,
        string? initialPrompt,
        TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        var status = car?.HasMinimumRequiredFields() == true
            ? ResearchStatus.Pending
            : ResearchStatus.Draft;
        var name = car?.HasAutoNameFields() == true
            ? $"{car.Make} {car.Model} {car.Year}"
            : null;

        return new ResearchRecord
        {
            Id = Guid.CreateVersion7(now),
            AuthId = authId,
            Name = name,
            Status = status,
            RiskLevel = null,
            InputMethod = inputMethod,
            Car = car,
            Description = description,
            DescriptionSource = descriptionSource,
            InitialPrompt = initialPrompt,
            CreditsSpent = 0,
            IsNameManual = false,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public ResearchRecord Rename(string name, TimeProvider timeProvider) =>
        this with
        {
            Name = name,
            IsNameManual = true,
            UpdatedAt = timeProvider.GetUtcNow()
        };
}
