using AutoVerdikt.Domain.Research;
using AutoVerdikt.WebApi.Endpoints.Research;
using Shouldly;

namespace AutoVerdikt.WebApi.Tests.Endpoints.Research;

public class ResearchDtoMappingsTests
{
    private static readonly Guid ResearchId = Guid.Parse("0192a1b2-c3d4-7890-abcd-ef1234567890");
    private const string AuthId = "auth_test";

    [Theory]
    [InlineData(DescriptionSource.AiGeneratedFromText, "aiGeneratedFromText")]
    [InlineData(DescriptionSource.AiGeneratedFromFacts, "aiGeneratedFromFacts")]
    public void ToDetailDto_MapsDescriptionSource(DescriptionSource descriptionSource, string expected)
    {
        var record = ResearchRecord.Create(
                AuthId,
                InputMethod.Form,
                new CarData { Make = "Volkswagen", Model = "Golf", Year = 2018 },
                "Well maintained car",
                descriptionSource,
                null,
                TimeProvider.System)
            with { Id = ResearchId };

        var dto = record.ToDetailDto();

        dto.DescriptionSource.ShouldBe(expected);
    }
}
