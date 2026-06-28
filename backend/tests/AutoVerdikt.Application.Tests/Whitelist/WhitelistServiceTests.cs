using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Application.Whitelist;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace AutoVerdikt.Application.Tests.Whitelist;

public sealed class WhitelistServiceTests
{
    private readonly Mock<IWhitelistRepository> _repository = new();
    private readonly Mock<IFeatureFlagService> _featureFlags = new();
    private readonly HybridCache _cache = CreateCache();

    [Fact]
    public async Task HasAccessAsync_FlagDisabled_ReturnsTrueWithoutRepositoryCall()
    {
        _featureFlags.Setup(f => f.IsWhitelistEnabled()).Returns(false);
        var service = CreateService();

        var result = await service.HasAccessAsync("auth_1");

        result.ShouldBeTrue();
        _repository.Verify(
            r => r.ExistsByAuthIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task IsWhitelistedAsync_Exists_ReturnsTrue()
    {
        _repository.Setup(r => r.ExistsByAuthIdAsync("auth_1", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var service = CreateService();

        (await service.IsWhitelistedAsync("auth_1")).ShouldBeTrue();
    }

    [Fact]
    public async Task IsWhitelistedAsync_NotExists_ReturnsFalse()
    {
        _repository.Setup(r => r.ExistsByAuthIdAsync("auth_1", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var service = CreateService();

        (await service.IsWhitelistedAsync("auth_1")).ShouldBeFalse();
    }

    [Fact]
    public async Task RemoveAsync_InvalidatesCache()
    {
        _repository.Setup(r => r.ExistsByAuthIdAsync("auth_1", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repository.Setup(r => r.RemoveByAuthIdAsync("auth_1", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var service = CreateService();

        (await service.IsWhitelistedAsync("auth_1")).ShouldBeTrue();
        (await service.RemoveAsync("auth_1")).ShouldBeTrue();

        _repository.Setup(r => r.ExistsByAuthIdAsync("auth_1", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        (await service.IsWhitelistedAsync("auth_1")).ShouldBeFalse();
    }

    private WhitelistService CreateService() =>
        new(_repository.Object, _featureFlags.Object, _cache, TimeProvider.System);

    private static HybridCache CreateCache()
    {
        var services = new ServiceCollection();
        services.AddHybridCache();
        return services.BuildServiceProvider().GetRequiredService<HybridCache>();
    }
}
