using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Store;
using AutoVerdikt.WebApi.Authentication.Clerk;
using AutoVerdikt.WebApi.Endpoints;
using AutoVerdikt.WebApi.Endpoints.Users;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);
var isTesting = builder.Environment.IsEnvironment("Testing");

builder.Services.AddOpenApi();

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
});

builder.Services.AddStore(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors();
app.UseClerkAuthentication();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet(HealthEndpoint.Route, () => Results.Ok(new { status = HealthEndpoint.Status }))
    .AllowAnonymous()
    .WithName(HealthEndpoint.Name);

app.MapUserEndpoints();

app.Run();
