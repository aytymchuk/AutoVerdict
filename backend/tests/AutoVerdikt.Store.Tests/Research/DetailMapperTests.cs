using AutoVerdikt.Domain.Research;
using AutoVerdikt.Store.Research;
using Shouldly;

namespace AutoVerdikt.Store.Tests.Research;

public class DetailMapperTests
{
    private static readonly Guid DetailId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567892");
    private static readonly Guid ResearchId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567891");
    private const string AuthId = "auth_test";

    [Fact]
    public void ToDocument_PreservesId()
    {
        var detail = new Detail
        {
            Id = DetailId,
            Name = "Horsepower",
            Value = "150 hp"
        };

        var document = DetailMapper.ToDocument(detail, ResearchId, AuthId);

        document.Id.ShouldBe(DetailId);
    }

    [Fact]
    public void ToDocument_SetsResearchIdAndAuthId()
    {
        var detail = new Detail
        {
            Id = DetailId,
            Name = "Horsepower",
            Value = "150 hp"
        };

        var document = DetailMapper.ToDocument(detail, ResearchId, AuthId);

        document.ResearchId.ShouldBe(ResearchId);
        document.AuthId.ShouldBe(AuthId);
    }

    [Fact]
    public void ToDomain_RoundTripsAllFields()
    {
        var original = new Detail
        {
            Id = DetailId,
            Name = "Horsepower",
            Value = "150 hp"
        };

        var document = DetailMapper.ToDocument(original, ResearchId, AuthId);
        var roundTripped = DetailMapper.ToDomain(document);

        roundTripped.Id.ShouldBe(original.Id);
        roundTripped.Name.ShouldBe(original.Name);
        roundTripped.Value.ShouldBe(original.Value);
    }
}
