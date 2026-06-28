using AutoVerdikt.Application.Research.Create;
using Microsoft.Extensions.DependencyInjection;

namespace AutoVerdikt.Application.Research;

public static class ResearchServiceExtensions
{
    public static IServiceCollection AddResearchServices(this IServiceCollection services)
    {
        services.AddSingleton<IStartResearchCommandFactory, StartResearchCommandFactory>();

        return services;
    }
}
