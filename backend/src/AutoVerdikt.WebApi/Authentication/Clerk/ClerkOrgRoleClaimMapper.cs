using System.Security.Claims;
using System.Text.Json;

namespace AutoVerdikt.WebApi.Authentication.Clerk;

internal static class ClerkOrgRoleClaimMapper
{
    private const string OrgClaimType = "o";
    private const string OrgRoleProperty = "rol";
    private const string LegacyOrgRoleClaimType = "org_role";
    private const string OrgRolePrefix = "org:";

    internal static void MapOrgRoleClaims(ClaimsPrincipal principal)
    {
        foreach (var identity in principal.Identities.OfType<ClaimsIdentity>())
        {
            if (!identity.IsAuthenticated)
                continue;

            foreach (var role in ExtractOrgRoles(principal))
                identity.AddClaim(new Claim(identity.RoleClaimType, role));
        }
    }

    internal static IEnumerable<string> ExtractOrgRoles(ClaimsPrincipal principal)
    {
        var roles = new HashSet<string>(StringComparer.Ordinal);

        var legacyRole = principal.FindFirst(LegacyOrgRoleClaimType)?.Value;
        if (!string.IsNullOrWhiteSpace(legacyRole))
            AddRoleVariants(roles, legacyRole);

        var flatRole = principal.FindFirst($"{OrgClaimType}.{OrgRoleProperty}")?.Value;
        if (!string.IsNullOrWhiteSpace(flatRole))
            AddRoleVariants(roles, flatRole);

        var orgJson = principal.FindFirst(OrgClaimType)?.Value;
        if (!string.IsNullOrWhiteSpace(orgJson))
        {
            using var doc = JsonDocument.Parse(orgJson);
            if (doc.RootElement.TryGetProperty(OrgRoleProperty, out var rolProp))
            {
                foreach (var role in ParseRoles(rolProp))
                    AddRoleVariants(roles, role);
            }
        }

        return roles;
    }

    private static void AddRoleVariants(ISet<string> roles, string role)
    {
        var trimmed = role.Trim();
        if (trimmed.Length == 0)
            return;

        roles.Add(trimmed);

        if (trimmed.StartsWith(OrgRolePrefix, StringComparison.Ordinal))
            roles.Add(trimmed[OrgRolePrefix.Length..]);
        else
            roles.Add($"{OrgRolePrefix}{trimmed}");
    }

    private static IEnumerable<string> ParseRoles(JsonElement rolProp) =>
        rolProp.ValueKind switch
        {
            JsonValueKind.String => rolProp.GetString() is { Length: > 0 } r ? [r] : [],
            JsonValueKind.Array => rolProp.EnumerateArray()
                .Select(e => e.GetString())
                .OfType<string>()
                .Where(r => r.Length > 0),
            _ => [],
        };
}
