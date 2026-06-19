using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AutoVerdikt.WebApi.Authentication.Clerk;

public static class ClerkAuthenticationExtensions
{
    private const string AzpClaimType = "azp";
    private const string UnauthorizedPartyErrorMessage = "Unauthorized party.";

    public static IServiceCollection AddClerkAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var clerkOptions = configuration.GetSection(ClerkOptions.SectionName).Get<ClerkOptions>() ?? new ClerkOptions();
        var clerkAuthority = clerkOptions.Authority;
        ArgumentException.ThrowIfNullOrEmpty(clerkAuthority);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => ConfigureClerkJwtBearer(options, clerkAuthority, clerkOptions));

        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }

    private static void ConfigureClerkJwtBearer(
        JwtBearerOptions options,
        string clerkAuthority,
        ClerkOptions clerkOptions)
    {
        options.Authority = clerkAuthority;
        options.TokenValidationParameters = CreateTokenValidationParameters(clerkAuthority);

        var authorizedParties = BuildAuthorizedParties(clerkOptions);
        options.Events = CreateJwtBearerEvents(authorizedParties);
    }

    private static TokenValidationParameters CreateTokenValidationParameters(string clerkAuthority) => new()
    {
        ValidateIssuer = true,
        ValidIssuer = clerkAuthority,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromSeconds(30),
    };

    private static HashSet<string> BuildAuthorizedParties(ClerkOptions clerkOptions)
    {
        var parties = new HashSet<string>(StringComparer.Ordinal);

        var authorizedParty = clerkOptions.AuthorizedParty;
        if (!string.IsNullOrEmpty(authorizedParty) && authorizedParty != "*")
            parties.Add(authorizedParty);

        foreach (var party in clerkOptions.AdditionalAuthorizedParties)
        {
            if (!string.IsNullOrWhiteSpace(party))
                parties.Add(party.Trim());
        }

        return parties;
    }

    private static JwtBearerEvents CreateJwtBearerEvents(HashSet<string> authorizedParties) => new()
    {
        OnMessageReceived = OnJwtMessageReceived,
        OnAuthenticationFailed = OnJwtAuthenticationFailed,
        OnTokenValidated = context => OnJwtTokenValidated(context, authorizedParties),
        OnChallenge = OnJwtChallenge,
    };

    private static Task OnJwtMessageReceived(MessageReceivedContext context)
    {
        var logger = GetLogger(context.HttpContext.RequestServices);
        var hasHeader = context.Request.Headers.ContainsKey("Authorization");
        ClerkAuthLog.JwtReceived(logger, hasHeader);
        return Task.CompletedTask;
    }

    private static Task OnJwtAuthenticationFailed(AuthenticationFailedContext context)
    {
        var logger = GetLogger(context.HttpContext.RequestServices);
        ClerkAuthLog.JwtAuthenticationFailed(logger, context.Exception.Message);
        return Task.CompletedTask;
    }

    private static Task OnJwtTokenValidated(TokenValidatedContext context, HashSet<string> authorizedParties)
    {
        ValidateAuthorizedParty(context, authorizedParties);

        if (context.Result?.Succeeded == false)
            return Task.CompletedTask;

        if (context.Principal is ClaimsPrincipal principal)
            ClerkOrgRoleClaimMapper.MapOrgRoleClaims(principal);

        return Task.CompletedTask;
    }

    private static void ValidateAuthorizedParty(TokenValidatedContext context, HashSet<string> authorizedParties)
    {
        if (authorizedParties.Count == 0) return;

        var azp = context.Principal?.FindFirst(AzpClaimType)?.Value;
        if (azp is not null && authorizedParties.Contains(azp)) return;

        var logger = GetLogger(context.HttpContext.RequestServices);
        ClerkAuthLog.JwtAzpRejected(logger, azp);
        context.Fail(UnauthorizedPartyErrorMessage);
    }

    private static Task OnJwtChallenge(JwtBearerChallengeContext context)
    {
        var logger = GetLogger(context.HttpContext.RequestServices);
        ClerkAuthLog.JwtChallenge(
            logger,
            context.Request.Path,
            context.AuthenticateFailure?.Message ?? "none");
        return Task.CompletedTask;
    }

    private static ILogger GetLogger(IServiceProvider services) =>
        services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(ClerkAuthenticationExtensions));

    public static IApplicationBuilder UseClerkAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
