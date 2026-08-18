using AutoVerdikt.Domain.Research;
using Shouldly;

namespace AutoVerdikt.Domain.Tests.Research;

public class AttachedFileTests
{
    [Fact]
    public void Create_SetsRequiredFieldsAndDefaults()
    {
        var file = AttachedFile.Create(
            FileKind.Photo,
            "listing.jpg",
            "https://storage.example.com/listing.jpg",
            TimeProvider.System);

        file.Kind.ShouldBe(FileKind.Photo);
        file.FileName.ShouldBe("listing.jpg");
        file.Url.ShouldBe("https://storage.example.com/listing.jpg");
        file.ThumbnailUrl.ShouldBeNull();
        file.AgentDescription.ShouldBeNull();
        file.ProcessingStatus.ShouldBe(ProcessingStatus.Pending);
        file.RejectionReason.ShouldBeNull();
    }

    [Fact]
    public void Create_WithAllOptionalFields_SetsThemAll()
    {
        var file = AttachedFile.Create(
            FileKind.Document,
            "report.pdf",
            "https://storage.example.com/report.pdf",
            TimeProvider.System,
            thumbnailUrl: "https://storage.example.com/report-thumb.jpg",
            agentDescription: "Service history document",
            processingStatus: ProcessingStatus.Completed,
            rejectionReason: null);

        file.ThumbnailUrl.ShouldBe("https://storage.example.com/report-thumb.jpg");
        file.AgentDescription.ShouldBe("Service history document");
        file.ProcessingStatus.ShouldBe(ProcessingStatus.Completed);
    }

    [Fact]
    public void Create_AssignsUuidV7Id()
    {
        var file = AttachedFile.Create(
            FileKind.Photo,
            "listing.jpg",
            "https://storage.example.com/listing.jpg",
            TimeProvider.System);

        file.Id.ToString()[14].ShouldBe('7');
    }
}
