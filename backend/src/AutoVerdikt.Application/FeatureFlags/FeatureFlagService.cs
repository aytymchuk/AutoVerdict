using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutoVerdikt.Application.FeatureFlags;

public sealed class FeatureFlagService(
    IOptions<FeatureFlagOptions> options,
    ILogger<FeatureFlagService> logger) : IFeatureFlagService
{
    private readonly bool _enabled = options.Value.WhitelistEnabled;

    public bool IsWhitelistEnabled() => _enabled;

    public void LogResolvedValue()
    {
        logger.LogInformation(
            "[FeatureFlag] WHITELIST_ENABLED = {Enabled} | {Description}",
            _enabled,
            _enabled
                ? "Access restricted to whitelist"
                : "All authenticated users have access");
    }
}
