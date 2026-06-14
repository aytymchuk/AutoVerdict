namespace AutoVerdikt.Application.FeatureFlags;

public sealed class FeatureFlagOptions
{
    public const string SectionName = "FeatureFlags";
    public bool WhitelistEnabled { get; init; }
}
