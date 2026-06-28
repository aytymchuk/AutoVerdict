using AutoVerdikt.Domain.Research;
using Shouldly;

namespace AutoVerdikt.Domain.Tests.Research;

public class ResearchRecordTests
{
    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    [Fact]
    public void Create_PartialCarData_SetsStatusDraft()
    {
        var car = new CarData
        {
            Make = "Volkswagen",
            Model = "Golf",
            Year = 2018
        };

        var record = ResearchRecord.Create(
            "auth_abc",
            InputMethod.Form,
            car,
            description: null,
            descriptionSource: null,
            TimeProvider.System);

        record.Status.ShouldBe(ResearchStatus.Draft);
    }

    [Fact]
    public void Create_AllMinimumFields_SetsStatusPending()
    {
        var car = new CarData
        {
            Make = "Volkswagen",
            Model = "Golf",
            Year = 2018,
            MileageKm = 87200,
            Price = 42900
        };

        var record = ResearchRecord.Create(
            "auth_abc",
            InputMethod.Form,
            car,
            description: null,
            descriptionSource: null,
            TimeProvider.System);

        record.Status.ShouldBe(ResearchStatus.Pending);
    }

    [Fact]
    public void Create_MakeModelYear_AutoGeneratesName()
    {
        var car = new CarData
        {
            Make = "Volkswagen",
            Model = "Golf",
            Year = 2018
        };

        var record = ResearchRecord.Create(
            "auth_abc",
            InputMethod.Form,
            car,
            description: null,
            descriptionSource: null,
            TimeProvider.System);

        record.Name.ShouldBe("Volkswagen Golf 2018");
        record.IsNameManual.ShouldBeFalse();
    }

    [Fact]
    public void Rename_SetsManualNameAndUpdatedAt()
    {
        var fixedTime = new DateTimeOffset(2026, 6, 19, 10, 42, 0, TimeSpan.Zero);
        var laterTime = new DateTimeOffset(2026, 6, 19, 10, 44, 12, TimeSpan.Zero);
        var timeProvider = new FixedTimeProvider(fixedTime);

        var record = ResearchRecord.Create(
            "auth_abc",
            InputMethod.Form,
            new CarData { Make = "Volkswagen", Model = "Golf", Year = 2018 },
            description: null,
            descriptionSource: null,
            timeProvider);

        var renamed = record.Rename("My Custom Name", new FixedTimeProvider(laterTime));

        renamed.Name.ShouldBe("My Custom Name");
        renamed.IsNameManual.ShouldBeTrue();
        renamed.UpdatedAt.ShouldBe(laterTime);
        record.Name.ShouldBe("Volkswagen Golf 2018");
        record.IsNameManual.ShouldBeFalse();
    }
}
