using AutoVerdikt.Domain.Research;
using AutoVerdikt.Store.Research;
using Shouldly;

namespace AutoVerdikt.Store.Tests.Research;

public class ResearchDocumentMapperTests
{
    private static readonly Guid ResearchId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567890");
    private const string AuthId = "auth_test";

    [Fact]
    public void ToDocument_ToDomain_RoundTripsCoreFields()
    {
        var createdAt = new DateTimeOffset(2026, 3, 1, 12, 0, 0, TimeSpan.Zero);
        var updatedAt = new DateTimeOffset(2026, 3, 10, 14, 30, 0, TimeSpan.Zero);
        var lastAnalyzedAt = new DateTimeOffset(2026, 3, 10, 14, 0, 0, TimeSpan.Zero);

        var record = ResearchRecord.Create(
                AuthId,
                InputMethod.Form,
                new CarData { Make = "Volkswagen", Model = "Golf", Year = 2018 },
                "Well maintained car",
                DescriptionSource.AiGeneratedFromFacts,
                "Analyze this listing",
                TimeProvider.System)
            with
            {
                Id = ResearchId,
                Name = "Volkswagen Golf 2018",
                Status = ResearchStatus.Analyzed,
                RiskLevel = RiskLevel.Medium,
                CreditsSpent = 5,
                IsNameManual = true,
                LastAnalyzedAt = lastAnalyzedAt,
                RetentionPolicy = "standard",
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };

        var document = ResearchDocumentMapper.ToDocument(record);
        var roundTripped = ResearchDocumentMapper.ToDomain(document, [], [], [], [], []);

        roundTripped.Id.ShouldBe(record.Id);
        roundTripped.AuthId.ShouldBe(record.AuthId);
        roundTripped.Name.ShouldBe(record.Name);
        roundTripped.Status.ShouldBe(record.Status);
        roundTripped.RiskLevel.ShouldBe(record.RiskLevel);
        roundTripped.InputMethod.ShouldBe(record.InputMethod);
        roundTripped.Car!.Make.ShouldBe(record.Car!.Make);
        roundTripped.Car.Model.ShouldBe(record.Car.Model);
        roundTripped.Car.Year.ShouldBe(record.Car.Year);
        roundTripped.Description.ShouldBe(record.Description);
        roundTripped.DescriptionSource.ShouldBe(DescriptionSource.AiGeneratedFromFacts);
        roundTripped.InitialPrompt.ShouldBe(record.InitialPrompt);
        roundTripped.CreditsSpent.ShouldBe(record.CreditsSpent);
        roundTripped.IsNameManual.ShouldBe(record.IsNameManual);
        roundTripped.LastAnalyzedAt.ShouldBe(record.LastAnalyzedAt);
        roundTripped.RetentionPolicy.ShouldBe(record.RetentionPolicy);
        roundTripped.CreatedAt.ShouldBe(record.CreatedAt);
        roundTripped.UpdatedAt.ShouldBe(record.UpdatedAt);
    }

    [Fact]
    public void ToDomainListItem_LeavesChildListsEmpty()
    {
        var record = CreateBaseRecord();
        var document = ResearchDocumentMapper.ToDocument(record);

        var listItem = ResearchDocumentMapper.ToDomainListItem(document);

        listItem.Notes.ShouldBeEmpty();
        listItem.Details.ShouldBeEmpty();
        listItem.Files.ShouldBeEmpty();
        listItem.Questions.ShouldBeEmpty();
        listItem.Badges.ShouldBeEmpty();
    }

    [Fact]
    public void ToDomain_AssemblesChildListsFromParameters()
    {
        var record = CreateBaseRecord();
        var document = ResearchDocumentMapper.ToDocument(record);

        var notes = new List<Note>
        {
            new()
            {
                Id = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567891"),
                Date = DateTimeOffset.UtcNow,
                Text = "Note 1"
            }
        };
        var details = new List<Detail>
        {
            new()
            {
                Id = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567892"),
                Name = "Mileage",
                Value = "87200 km"
            }
        };
        var files = new List<AttachedFile>
        {
            new()
            {
                Id = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567893"),
                Kind = FileKind.Photo,
                FileName = "photo.jpg",
                Url = "https://example.com/photo.jpg",
                ProcessingStatus = ProcessingStatus.Completed
            }
        };
        var questions = new List<Question>
        {
            new()
            {
                Id = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567894"),
                QuestionText = "Service history?",
                ReviewedByAssistant = false
            }
        };
        var badges = new List<Badge>
        {
            new()
            {
                Id = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567895"),
                Name = "Clean title",
                Status = BadgeStatus.Good,
                Description = "No liens"
            }
        };

        var result = ResearchDocumentMapper.ToDomain(document, notes, details, files, questions, badges);

        result.Notes.ShouldBeSameAs(notes);
        result.Details.ShouldBeSameAs(details);
        result.Files.ShouldBeSameAs(files);
        result.Questions.ShouldBeSameAs(questions);
        result.Badges.ShouldBeSameAs(badges);
    }

    [Fact]
    public void ToDomain_LegacyPasteInputMethod_ParsesAsText()
    {
        var document = CreateBaseDocument(inputMethod: "paste");

        var record = ResearchDocumentMapper.ToDomainListItem(document);

        record.InputMethod.ShouldBe(InputMethod.Text);
    }

    [Fact]
    public void ToDomain_UnknownStatus_DefaultsToDraft()
    {
        var document = CreateBaseDocument(status: "unknown");

        var record = ResearchDocumentMapper.ToDomainListItem(document);

        record.Status.ShouldBe(ResearchStatus.Draft);
    }

    [Fact]
    public void ToDomain_NullRiskLevel_RemainsNull()
    {
        var document = CreateBaseDocument(riskLevel: null);

        var record = ResearchDocumentMapper.ToDomainListItem(document);

        record.RiskLevel.ShouldBeNull();
    }

    private static ResearchRecord CreateBaseRecord() =>
        ResearchRecord.Create(
                AuthId,
                InputMethod.Form,
                new CarData { Make = "Volkswagen", Model = "Golf", Year = 2018 },
                null,
                null,
                null,
                TimeProvider.System)
            with { Id = ResearchId };

    private static ResearchDocument CreateBaseDocument(
        string? status = "pending",
        string? riskLevel = null,
        string inputMethod = "form")
    {
        var record = CreateBaseRecord();
        return new ResearchDocument
        {
            Id = record.Id,
            AuthId = record.AuthId,
            Name = record.Name,
            Status = status,
            RiskLevel = riskLevel,
            InputMethod = inputMethod,
            Car = record.Car is null ? null : CarDataMapper.ToDocument(record.Car),
            Description = record.Description,
            DescriptionSource = null,
            InitialPrompt = record.InitialPrompt,
            CreditsSpent = record.CreditsSpent,
            IsNameManual = record.IsNameManual,
            LastAnalyzedAt = null,
            RetentionPolicy = null,
            CreatedAt = record.CreatedAt.UtcDateTime,
            UpdatedAt = record.UpdatedAt.UtcDateTime
        };
    }
}
