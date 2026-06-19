using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

internal sealed class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Test";
    public const string UserIdHeaderName = "X-Test-UserId";
    public const string RoleHeaderName = "X-Test-Role";
    public const string EmailHeaderName = "X-Test-Email";
    public const string LocaleHeaderName = "X-Test-Locale";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(UserIdHeaderName, out var userIdValues)
            || string.IsNullOrWhiteSpace(userIdValues.FirstOrDefault()))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var userId = userIdValues.ToString();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };

        if (Request.Headers.TryGetValue(EmailHeaderName, out var emailValues)
            && !string.IsNullOrWhiteSpace(emailValues.FirstOrDefault()))
        {
            claims.Add(new Claim(ClaimTypes.Email, emailValues.ToString()));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.Email, $"{userId}@example.com"));
        }

        if (Request.Headers.TryGetValue(LocaleHeaderName, out var localeValues)
            && !string.IsNullOrWhiteSpace(localeValues.FirstOrDefault()))
        {
            claims.Add(new Claim("locale", localeValues.ToString()));
        }
        else
        {
            claims.Add(new Claim("locale", "en"));
        }

        if (Request.Headers.TryGetValue(RoleHeaderName, out var roleValues)
            && !string.IsNullOrWhiteSpace(roleValues.FirstOrDefault()))
        {
            claims.Add(new Claim(ClaimTypes.Role, roleValues.ToString()));
        }

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
