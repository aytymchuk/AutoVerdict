namespace AutoVerdikt.WebApi.Authorization;

internal static class AdminAuthorizationExtensions
{
    internal const string AdminPolicyName = "Admin";

    internal static IServiceCollection AddAdminAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AdminPolicyName, policy => policy.RequireRole("admin"));

        return services;
    }
}
