using AutoVerdikt.Application.FeatureFlags;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace AutoVerdikt.Application.Tests.FeatureFlags;

public sealed class FeatureFlagServiceTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsWhitelistEnabled_ReturnsConfiguredValue(bool enabled)
    {
        var service = CreateService(enabled);
        service.IsWhitelistEnabled().ShouldBe(enabled);
    }

    [Fact]
    public void IsWhitelistEnabled_DefaultFalse_WhenNotConfigured()
    {
        var service = CreateService(false);
        service.IsWhitelistEnabled().ShouldBeFalse();
    }

    private static FeatureFlagService CreateService(bool enabled)
    {
        var options = Options.Create(new FeatureFlagOptions { WhitelistEnabled = enabled });
        var logger = new Mock<ILogger<FeatureFlagService>>();
        return new FeatureFlagService(options, logger.Object);
    }
}
