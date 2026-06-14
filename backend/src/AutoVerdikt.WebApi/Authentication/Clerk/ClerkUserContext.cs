using System.Security.Claims;
using AutoVerdikt.Application.Abstractions;

namespace AutoVerdikt.WebApi.Authentication.Clerk;

internal sealed class ClerkUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    public string AuthId =>
        httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new InvalidOperationException("No authenticated user in the current context.");

    public string Email =>
        httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value
        ?? throw new InvalidOperationException("No email claim in the current context.");

    public string Locale =>
        httpContextAccessor.HttpContext?.User.FindFirst("locale")?.Value ?? "en";

    public bool IsAdmin =>
        httpContextAccessor.HttpContext?.User.IsInRole("admin") == true;
}
