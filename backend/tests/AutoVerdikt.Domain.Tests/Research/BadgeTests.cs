using AutoVerdikt.Domain.Research;
using Shouldly;

namespace AutoVerdikt.Domain.Tests.Research;

public class BadgeTests
{
    [Fact]
    public void Create_SetsRequiredFieldsAndDefaultsSourceToNull()
    {
        var badge = Badge.Create(
            "Full service history",
            BadgeStatus.Good,
            "Complete dealer records",
            TimeProvider.System);

        badge.Name.ShouldBe("Full service history");
        badge.Status.ShouldBe(BadgeStatus.Good);
        badge.Description.ShouldBe("Complete dealer records");
        badge.Source.ShouldBeNull();
    }

    [Fact]
    public void Create_WithSource_SetsSource()
    {
        var badge = Badge.Create(
            "Frontal impact",
            BadgeStatus.Critical,
            "Structural damage reported",
            TimeProvider.System,
            source: "analysis");

        badge.Source.ShouldBe("analysis");
    }

    [Fact]
    public void Create_AssignsUuidV7Id()
    {
        var badge = Badge.Create(
            "Full service history",
            BadgeStatus.Good,
            "Complete dealer records",
            TimeProvider.System);

        badge.Id.ToString()[14].ShouldBe('7');
    }

    [Theory]
    [InlineData(BadgeStatus.Good)]
    [InlineData(BadgeStatus.Warning)]
    [InlineData(BadgeStatus.Critical)]
    public void Create_PreservesBadgeStatus(BadgeStatus status)
    {
        var badge = Badge.Create("Test badge", status, "Test description", TimeProvider.System);

        badge.Status.ShouldBe(status);
    }
}
