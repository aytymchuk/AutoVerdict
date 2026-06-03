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
        var scalarOpts = app.ServiceProvider
            .GetRequiredService<IOptions<ScalarUiOptions>>()
            .Value;

        app.MapOpenApi().AllowAnonymous();

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("AutoVerdikt API")
                .EnablePersistentAuthentication()
                .AddPreferredSecuritySchemes(SecuritySchemeNames.ClerkOAuth2)
                .AddAuthorizationCodeFlow(SecuritySchemeNames.ClerkOAuth2, flow =>
                {
                    flow.ClientId = scalarOpts.ClientId;
                    flow.ClientSecret = scalarOpts.ClientSecret;
                    flow.WithPkce(Pkce.Sha256);
                    flow.WithSelectedScopes(ClerkScopes.All);

                    if (!string.IsNullOrEmpty(scalarOpts.AuthorizationUrl))
                        flow.WithAuthorizationUrl(scalarOpts.AuthorizationUrl);
                    if (!string.IsNullOrEmpty(scalarOpts.TokenUrl))
                        flow.WithTokenUrl(scalarOpts.TokenUrl);
                    // redirect_uri must be sent in the token exchange to match the authorization request.
                    if (!string.IsNullOrEmpty(scalarOpts.RedirectUri))
                        flow.WithRedirectUri(scalarOpts.RedirectUri);
                });
        }).AllowAnonymous(); // Must bypass global auth policy so the UI itself is reachable

        return app;
    }
}
