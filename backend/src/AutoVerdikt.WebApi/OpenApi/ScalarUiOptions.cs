namespace AutoVerdikt.WebApi.OpenApi;

public class ScalarUiOptions
{
    public const string SectionName = "Scalar";

    public string ClientId { get; set; } = string.Empty;
    public string? ClientSecret { get; set; }

    // Override defaults when Clerk's URL pattern differs from {Authority}/oauth/*
    public string? AuthorizationUrl { get; set; }
    public string? TokenUrl { get; set; }
}
