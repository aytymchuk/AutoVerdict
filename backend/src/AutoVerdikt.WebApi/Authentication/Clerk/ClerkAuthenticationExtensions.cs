using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AutoVerdikt.WebApi.Authentication.Clerk;

public static class ClerkAuthenticationExtensions
{
    public static IServiceCollection AddClerkAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var clerkAuthority = configuration["Clerk:Authority"]!;
        var authorizedParty = configuration["Clerk:AuthorizedParty"];

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Clerk's Frontend API URL — ASP.NET Core auto-discovers the JWKS
                // from {Authority}/.well-known/openid-configuration
                options.Authority = clerkAuthority;

                // Clerk tokens don't always include a standard 'aud' claim,
                // so disable audience validation (or set it if you've configured
                // a JWT template with a custom audience in Clerk Dashboard)
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = clerkAuthority,

                    ValidateAudience = false,   // Set to true if you have a custom JWT template with aud
                    // ValidAudience = "your-api",

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    // Clock skew to handle minor time drift
                    ClockSkew = TimeSpan.FromSeconds(30),
                };

                // Validate the azp claim (authorized party = your frontend origin)
                if (!string.IsNullOrEmpty(authorizedParty))
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var azp = context.Principal?.FindFirst("azp")?.Value;
                            if (azp is null || azp != authorizedParty)
                            {
                                context.Fail("Unauthorized party.");
                            }
                            return Task.CompletedTask;
                        }
                    };
                }
            });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        
        return services;
    }

    public static IApplicationBuilder UseClerkAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();    // Must come before UseAuthorization
        app.UseAuthorization();

        return app;
    }
}
