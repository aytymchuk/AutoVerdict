using AutoVerdikt.Domain.Research;
using AutoVerdikt.Store.Research;
using Shouldly;

namespace AutoVerdikt.Store.Tests.Research;

public class AttachedFileMapperTests
{
    private static readonly Guid FileId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567895");
    private static readonly Guid ResearchId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567891");
    private const string AuthId = "auth_test";

    [Fact]
    public void ToDocument_PreservesFileId()
    {
        var file = CreateSampleFile();

        var document = AttachedFileMapper.ToDocument(file, ResearchId, AuthId);

        document.Id.ShouldBe(FileId);
    }

    [Fact]
    public void ToDomain_RoundTripsAllFields()
    {
        var original = CreateSampleFile();

        var document = AttachedFileMapper.ToDocument(original, ResearchId, AuthId);
        var roundTripped = AttachedFileMapper.ToDomain(document);

        roundTripped.Id.ShouldBe(original.Id);
        roundTripped.Kind.ShouldBe(original.Kind);
        roundTripped.FileName.ShouldBe(original.FileName);
        roundTripped.Url.ShouldBe(original.Url);
        roundTripped.ThumbnailUrl.ShouldBe(original.ThumbnailUrl);
        roundTripped.AgentDescription.ShouldBe(original.AgentDescription);
        roundTripped.ProcessingStatus.ShouldBe(original.ProcessingStatus);
        roundTripped.RejectionReason.ShouldBe(original.RejectionReason);
    }

    [Theory]
    [InlineData(FileKind.Photo, "photo")]
    [InlineData(FileKind.Document, "document")]
    public void ToDocument_ToDomain_RoundTripsFileKind(FileKind kind, string expectedString)
    {
        var file = CreateSampleFile() with { Kind = kind };

        var document = AttachedFileMapper.ToDocument(file, ResearchId, AuthId);
        document.Kind.ShouldBe(expectedString);

        var roundTripped = AttachedFileMapper.ToDomain(document);
        roundTripped.Kind.ShouldBe(kind);
    }

    [Fact]
    public void ToDomain_UnknownFileKind_DefaultsToDocument()
    {
        var document = CreateSampleDocument(kind: "unknown");

        var file = AttachedFileMapper.ToDomain(document);

        file.Kind.ShouldBe(FileKind.Document);
    }

    [Theory]
    [InlineData(ProcessingStatus.Pending, "pending")]
    [InlineData(ProcessingStatus.Processing, "processing")]
    [InlineData(ProcessingStatus.Completed, "completed")]
    [InlineData(ProcessingStatus.Rejected, "rejected")]
    public void ToDocument_ToDomain_RoundTripsProcessingStatus(ProcessingStatus status, string expectedString)
    {
        var file = CreateSampleFile() with { ProcessingStatus = status };

        var document = AttachedFileMapper.ToDocument(file, ResearchId, AuthId);
        document.ProcessingStatus.ShouldBe(expectedString);

        var roundTripped = AttachedFileMapper.ToDomain(document);
        roundTripped.ProcessingStatus.ShouldBe(status);
    }

    [Fact]
    public void ToDomain_UnknownProcessingStatus_DefaultsToPending()
    {
        var document = CreateSampleDocument(processingStatus: "unknown");

        var file = AttachedFileMapper.ToDomain(document);

        file.ProcessingStatus.ShouldBe(ProcessingStatus.Pending);
    }

    private static AttachedFile CreateSampleFile() =>
        new()
        {
            Id = FileId,
            Kind = FileKind.Photo,
            FileName = "listing.jpg",
            Url = "https://storage.example.com/listing.jpg",
            ThumbnailUrl = "https://storage.example.com/listing-thumb.jpg",
            AgentDescription = "Front view of the car",
            ProcessingStatus = ProcessingStatus.Completed,
            RejectionReason = null
        };

    private static AttachedFileDocument CreateSampleDocument(
        string kind = "photo",
        string processingStatus = "completed") =>
        new()
        {
            Id = FileId,
            ResearchId = ResearchId,
            AuthId = AuthId,
            Kind = kind,
            FileName = "listing.jpg",
            Url = "https://storage.example.com/listing.jpg",
            ThumbnailUrl = "https://storage.example.com/listing-thumb.jpg",
            AgentDescription = "Front view of the car",
            ProcessingStatus = processingStatus
        };
}
