using AutoVerdikt.Domain.Research;
using AutoVerdikt.Store.Research;
using Shouldly;

namespace AutoVerdikt.Store.Tests.Research;

public class BadgeMapperTests
{
    private static readonly Guid BadgeId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567894");
    private static readonly Guid ResearchId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567891");
    private const string AuthId = "auth_test";

    [Fact]
    public void ToDocument_PreservesId()
    {
        var badge = new Badge
        {
            Id = BadgeId,
            Name = "Full service history",
            Status = BadgeStatus.Good,
            Description = "Complete dealer records",
            Source = "analysis"
        };

        var document = BadgeMapper.ToDocument(badge, ResearchId, AuthId);

        document.Id.ShouldBe(BadgeId);
    }

    [Fact]
    public void ToDocument_SetsResearchIdAndAuthId()
    {
        var badge = new Badge
        {
            Id = BadgeId,
            Name = "Frontal impact",
            Status = BadgeStatus.Critical,
            Description = "Structural damage reported",
            Source = null
        };

        var document = BadgeMapper.ToDocument(badge, ResearchId, AuthId);

        document.ResearchId.ShouldBe(ResearchId);
        document.AuthId.ShouldBe(AuthId);
    }

    [Fact]
    public void ToDomain_RoundTripsAllFields()
    {
        var original = new Badge
        {
            Id = BadgeId,
            Name = "Full service history",
            Status = BadgeStatus.Good,
            Description = "Complete dealer records",
            Source = "analysis"
        };

        var document = BadgeMapper.ToDocument(original, ResearchId, AuthId);
        var roundTripped = BadgeMapper.ToDomain(document);

        roundTripped.Id.ShouldBe(original.Id);
        roundTripped.Name.ShouldBe(original.Name);
        roundTripped.Status.ShouldBe(original.Status);
        roundTripped.Description.ShouldBe(original.Description);
        roundTripped.Source.ShouldBe(original.Source);
    }

    [Theory]
    [InlineData(BadgeStatus.Good, "good")]
    [InlineData(BadgeStatus.Warning, "warning")]
    [InlineData(BadgeStatus.Critical, "critical")]
    public void ToDocument_ToDomain_RoundTripsBadgeStatus(BadgeStatus status, string expectedString)
    {
        var badge = new Badge
        {
            Id = BadgeId,
            Name = "Test badge",
            Status = status,
            Description = "Test description"
        };

        var document = BadgeMapper.ToDocument(badge, ResearchId, AuthId);
        document.Status.ShouldBe(expectedString);

        var roundTripped = BadgeMapper.ToDomain(document);
        roundTripped.Status.ShouldBe(status);
    }

    [Fact]
    public void ToDomain_UnknownStatus_ThrowsArgumentOutOfRangeException()
    {
        var document = new BadgeDocument
        {
            Id = BadgeId,
            ResearchId = ResearchId,
            AuthId = AuthId,
            Name = "Test",
            Status = "unknown",
            Description = "Test"
        };

        var act = () => BadgeMapper.ToDomain(document);

        act.ShouldThrow<ArgumentOutOfRangeException>()
            .ParamName.ShouldBe("value");
    }
}
