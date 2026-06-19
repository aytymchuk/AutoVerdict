namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public sealed class WhitelistDisabledFactory : AutoVerdiktWebApiFactory
{
    public WhitelistDisabledFactory() => WhitelistEnabled = false;
}
