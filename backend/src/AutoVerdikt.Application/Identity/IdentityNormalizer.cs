namespace AutoVerdikt.Application.Identity;

public static class IdentityNormalizer
{
    public static string NormalizeAuthId(string authId) => authId.Trim();

    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
