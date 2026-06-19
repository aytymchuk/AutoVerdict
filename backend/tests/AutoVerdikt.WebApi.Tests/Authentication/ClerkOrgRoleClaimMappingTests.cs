using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoVerdikt.WebApi.Authentication.Clerk;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shouldly;

namespace AutoVerdikt.WebApi.Tests.Authentication;

public sealed class ClerkOrgRoleClaimMappingTests
{
    [Fact]
    public async Task JsonWebTokenHandler_StoresNestedOrgClaim_AsJsonString()
    {
        var principal = await ValidateClerkLikeToken("""
            {
              "azp": "http://localhost:5173",
              "o": {"id":"org_123","rol":"admin","slg":"test"},
              "sub": "user_abc"
            }
            """);

        principal.FindFirst("o")?.Value.ShouldBe("""{"id":"org_123","rol":"admin","slg":"test"}""");
    }

    [Fact]
    public void MapOrgRoleClaims_AddsAdminRole_FromNestedOrgClaim()
    {
        var principal = CreatePrincipal("""{"id":"org_123","rol":"admin","slg":"test"}""");

        ClerkOrgRoleClaimMapper.MapOrgRoleClaims(principal);

        principal.IsInRole("admin").ShouldBeTrue();
        principal.IsInRole("org:admin").ShouldBeTrue();
    }

    [Fact]
    public void MapOrgRoleClaims_AddsAdminRole_FromLegacyOrgRoleClaim()
    {
        var identity = new ClaimsIdentity(authenticationType: "Bearer");
        identity.AddClaim(new Claim("org_role", "org:admin"));
        var principal = new ClaimsPrincipal(identity);

        ClerkOrgRoleClaimMapper.MapOrgRoleClaims(principal);

        principal.IsInRole("admin").ShouldBeTrue();
        principal.IsInRole("org:admin").ShouldBeTrue();
    }

    [Fact]
    public void ExtractOrgRoles_ReturnsEmpty_WhenNoOrgClaimsPresent()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(authenticationType: "Bearer"));

        ClerkOrgRoleClaimMapper.ExtractOrgRoles(principal).ShouldBeEmpty();
    }

    private static ClaimsPrincipal CreatePrincipal(string orgJson)
    {
        var identity = new ClaimsIdentity(authenticationType: "Bearer");
        identity.AddClaim(new Claim("o", orgJson));
        return new ClaimsPrincipal(identity);
    }

    private static async Task<ClaimsPrincipal> ValidateClerkLikeToken(string jsonPayload)
    {
        var token = CreateUnsignedToken(jsonPayload);
        var handler = new JsonWebTokenHandler();
        var validation = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            SignatureValidator = (jwt, _) => new JsonWebToken(jwt),
        };

        var result = await handler.ValidateTokenAsync(token, validation);
        return new ClaimsPrincipal(result.ClaimsIdentity);
    }

    private static string CreateUnsignedToken(string jsonPayload)
    {
        var header = Base64UrlEncoder.Encode(
            Encoding.UTF8.GetBytes("""{"alg":"none","typ":"JWT"}"""));
        var payload = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(jsonPayload));
        return $"{header}.{payload}.";
    }
}
