namespace AutoVerdikt.WebApi.OpenApi;

public class ScalarUiOptions
{
    public const string SectionName = "Scalar";

    // Override defaults when Clerk's URL pattern differs from {Authority}/oauth/*
    public string? AuthorizationUrl { get; set; }
    public string? TokenUrl { get; set; }
}
