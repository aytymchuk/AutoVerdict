using AutoVerdikt.WebApi.Authentication.Clerk;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace AutoVerdikt.WebApi.OpenApi;

internal sealed class SecuritySchemeDocumentTransformer(
    IOptions<ClerkOptions> clerkOptions,
    IOptions<ScalarUiOptions> scalarOptions) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authority = clerkOptions.Value.Authority.TrimEnd('/');
        var opts = scalarOptions.Value;

        var authUrl = opts.AuthorizationUrl ?? $"{authority}/oauth/authorize";
        var tokenUrl = opts.TokenUrl ?? $"{authority}/oauth/token";

        var components = document.Components ??= new OpenApiComponents();

        components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            [SecuritySchemeNames.ClerkOAuth2] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authUrl),
                        TokenUrl = new Uri(tokenUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            ["openid"] = "OpenID Connect",
                            ["profile"] = "User profile",
                            ["email"] = "Email address"
                        }
                    }
                }
            },
            [SecuritySchemeNames.Bearer] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Clerk JWT session token (Bearer)"
            }
        };

        return Task.CompletedTask;
    }
}
