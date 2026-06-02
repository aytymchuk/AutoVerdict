using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace AutoVerdikt.WebApi.OpenApi;

internal sealed class SecurityRequirementOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        var allowAnonymous = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<IAllowAnonymous>()
            .Any();

        if (!allowAnonymous)
        {
            // Only ClerkOAuth2 — Scalar stores the access token on the OAuth flow.
            // Listing Bearer as an OR alternative makes Scalar pick it and omit the token.
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(SecuritySchemeNames.ClerkOAuth2, context.Document)] = []
                }
            ];
        }

        return Task.CompletedTask;
    }
}
