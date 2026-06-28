using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Behaviors;
using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Infrastructure;
using AutoVerdikt.Store;
using AutoVerdikt.WebApi;
using AutoVerdikt.WebApi.Authentication.Clerk;
using AutoVerdikt.WebApi.Authorization;
using AutoVerdikt.WebApi.Endpoints;
using AutoVerdikt.WebApi.Endpoints.Admin;
using AutoVerdikt.WebApi.Endpoints.Product;
using AutoVerdikt.WebApi.Endpoints.Research;
using AutoVerdikt.WebApi.Endpoints.Users;
using AutoVerdikt.WebApi.Endpoints.Whitelist;
using AutoVerdikt.WebApi.Exceptions;
using AutoVerdikt.WebApi.Observability;
using AutoVerdikt.WebApi.OpenApi;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);
var isTesting = builder.Environment.IsEnvironment("Testing");

ApplyWhitelistEnabledEnvironmentOverride(builder.Configuration);

builder.AddObservability();

builder.Services.AddScalarOpenApi(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

if (!isTesting)
{
    builder.Services.AddClerkAuthentication(builder.Configuration);
}

var clerkOptions = builder.Configuration.GetSection(ClerkOptions.SectionName).Get<ClerkOptions>() ?? new ClerkOptions();
var authorizedParty = clerkOptions.AuthorizedParty;

if (!builder.Environment.IsDevelopment() && !isTesting)
{
    ArgumentException.ThrowIfNullOrEmpty(authorizedParty);
    if (authorizedParty == "*")
    {
        throw new InvalidOperationException("AuthorizedParty is wildcard '*' in production, which is not permitted.");
    }
}

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
{
    if ((builder.Environment.IsDevelopment() || isTesting)
        && (string.IsNullOrEmpty(authorizedParty) || authorizedParty == "*"))
    {
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    }
    else
    {
        p.WithOrigins(authorizedParty!)
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials();
    }
}));

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, ClerkUserContext>();
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.PipelineBehaviors = [typeof(LoggingPipelineBehavior<,>)];
});

builder.Services.AddApplicationServices();
builder.Services.AddStore(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAdminAuthorization();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.Services.GetRequiredService<IFeatureFlagService>()
    .LogResolvedValue();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors();
app.UseClerkAuthentication();

if (app.Environment.IsDevelopment())
{
    app.MapScalarUi();
}

app.MapGet(HealthEndpoint.Route, () => Results.Ok(new { status = HealthEndpoint.Status }))
    .AllowAnonymous()
    .WithName(HealthEndpoint.Name);

app.MapUserEndpoints();
app.MapResearchEndpoints();
app.MapWaitlistRequestEndpoints();
app.MapProductEndpoints();
app.MapWhitelistAdminEndpoints();

app.Run();

static void ApplyWhitelistEnabledEnvironmentOverride(ConfigurationManager configuration)
{
    var envValue = Environment.GetEnvironmentVariable("WHITELIST_ENABLED");
    if (string.IsNullOrWhiteSpace(envValue))
        return;

    var enabled = string.Equals(envValue, "true", StringComparison.OrdinalIgnoreCase);
    configuration[$"{FeatureFlagOptions.SectionName}:WhitelistEnabled"] = enabled.ToString().ToLowerInvariant();
}
