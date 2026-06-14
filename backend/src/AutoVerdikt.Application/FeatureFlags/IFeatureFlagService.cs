namespace AutoVerdikt.Application.FeatureFlags;

public interface IFeatureFlagService
{
    bool IsWhitelistEnabled();
    void LogResolvedValue();
}
