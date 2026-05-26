# WebApi Layer

The composition root. Wires all layers together via Dependency Injection and exposes the HTTP API.

## Security

- Authentication is **required by default** for all endpoints via a global fallback policy using Clerk JWTs.
- Use `.AllowAnonymous()` explicitly only for intentionally public endpoints (e.g., health check).
- Extract the authenticated user's identity through `ICurrentUserContext` — never read JWT claims directly inside handlers or endpoints.

## Endpoint Conventions

- **Constants**: All route paths, endpoint names, and fixed response values must be defined as `internal const` in a dedicated `*EndpointConstants` class inside the same `Endpoints/<Feature>/` folder. Never inline them as string literals.
- **DTOs**: Request and response DTOs live in `Endpoints/<Feature>/`. Keep them minimal — never expose internal identifiers (e.g., `AuthId`) in response DTOs.
- **Endpoint mapping**: Define a `Map*Endpoints(this IEndpointRouteBuilder app)` extension method per feature and call it from `Program.cs`.

## HTTP Request Validation

- Validate all request DTOs with **FluentValidation** (`AbstractValidator<TDto>`).
- The validator class lives in the same `Endpoints/<Feature>/` folder as its DTO, named `<Dto>Validator`.
- Wire validation into the pipeline by chaining `.AddFluentValidationAutoValidation()` on each `RouteHandlerBuilder` — validation runs before the handler body executes.
- Register at startup: `AddValidatorsFromAssemblyContaining<Program>()` + `AddFluentValidationAutoValidation()`.
- **Never** perform manual null/format checks inside the endpoint handler — delegate all input validation to the FluentValidation validator.

## Configuration

- Use `IOptions<T>` for all typed configuration. Never read `IConfiguration` by string key directly inside application code.
