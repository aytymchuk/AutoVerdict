using AutoVerdikt.WebApi.Authentication.Clerk;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace AutoVerdikt.WebApi.OpenApi;

internal static class OpenApiExtensions
{
    internal static IServiceCollection AddScalarOpenApi(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ClerkOptions>(
            configuration.GetSection(ClerkOptions.SectionName));
        services.Configure<ScalarUiOptions>(
            configuration.GetSection(ScalarUiOptions.SectionName));

        services.AddTransient<SecuritySchemeDocumentTransformer>();
        services.AddTransient<SecurityRequirementOperationTransformer>();

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<SecuritySchemeDocumentTransformer>();
            options.AddOperationTransformer<SecurityRequirementOperationTransformer>();
        });

        return services;
    }

    internal static IEndpointRouteBuilder MapScalarUi(
        this IEndpointRouteBuilder app)
    {
        var clientId = app.ServiceProvider
            .GetRequiredService<IOptions<ClerkOptions>>()
            .Value.ClientId ?? string.Empty;

        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("AutoVerdikt API")
                .WithPreferredScheme(SecuritySchemeNames.ClerkOAuth2)
                .WithOAuth2Authentication(oauth2 =>
                {
                    oauth2.ClientId = clientId;
                });
        }).AllowAnonymous(); // Must bypass global auth policy so the UI itself is reachable

        return app;
    }
}
