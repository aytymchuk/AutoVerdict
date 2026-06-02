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
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = SecuritySchemeNames.ClerkOAuth2, Type = ReferenceType.SecurityScheme } }] = []
                },
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = SecuritySchemeNames.Bearer, Type = ReferenceType.SecurityScheme } }] = []
                }
            ];
        }

        return Task.CompletedTask;
    }
}
