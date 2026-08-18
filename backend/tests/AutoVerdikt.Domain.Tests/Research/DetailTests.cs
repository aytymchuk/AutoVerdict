using AutoVerdikt.Domain.Research;
using Shouldly;

namespace AutoVerdikt.Domain.Tests.Research;

public class DetailTests
{
    [Fact]
    public void Create_SetsAllPropertiesFromInputs()
    {
        var detail = Detail.Create("Horsepower", "150 hp", TimeProvider.System);

        detail.Name.ShouldBe("Horsepower");
        detail.Value.ShouldBe("150 hp");
    }

    [Fact]
    public void Create_AssignsUuidV7Id()
    {
        var detail = Detail.Create("Horsepower", "150 hp", TimeProvider.System);

        detail.Id.ToString()[14].ShouldBe('7');
    }
}
