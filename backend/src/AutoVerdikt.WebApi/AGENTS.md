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
- **Thin endpoints**: An endpoint handler must contain only: (1) mapping the incoming DTO to a command or query, (2) a single `mediator.Send(...)` call, and (3) mapping the result to an HTTP response via `ToProblemResult()` or `Results.*`. No business logic, input method branching, data transformation, or conditional domain decisions belong in an endpoint — they belong in the Application layer.

## OpenAPI Metadata

- **Document every endpoint** for the generated OpenAPI/Scalar docs by chaining metadata on the `RouteHandlerBuilder`:
  - `.WithName(...)` — operation id.
  - `.WithSummary(...)` and `.WithDescription(...)` — human-readable docs.
  - `.Produces<TDto>(StatusCodes.Status2xx)` for every success response, and `.Produces(StatusCodes.Status4xx)` / `.ProducesValidationProblem()` for every failure the handler can return (e.g. `401`, `404`, `409`, validation `400`).
- **No magic strings**: summary and description text must be `internal const` in the feature's `*EndpointConstants` class — never inlined. Pass `StatusCodes.*` constants rather than raw numbers.
- Keep the declared responses in sync with what the handler actually returns; every `Results.*` branch should have a matching `.Produces*` entry.

## HTTP Request Validation

- Validate all request DTOs with **FluentValidation** (`AbstractValidator<TDto>`).
- The validator class lives in the same `Endpoints/<Feature>/` folder as its DTO, named `<Dto>Validator`.
- Wire validation into the pipeline by chaining `.AddFluentValidationAutoValidation()` on each `RouteHandlerBuilder` — validation runs before the handler body executes.
- Register at startup: `AddValidatorsFromAssemblyContaining<Program>()` + `AddFluentValidationAutoValidation()`.
- **Never** perform manual null/format checks inside the endpoint handler — delegate all input validation to the FluentValidation validator.

## Error Mapping

- **Use typed error checks**: When translating a `Result` failure into an HTTP response, use
  `result.Errors.OfType<TError>().FirstOrDefault()` to detect specific error types. Never match
  by error message string, substring, or metadata key — these are fragile and break silently on
  wording changes.

  ```csharp
  // Correct — refactor-safe
  var alreadyRegistered = result.Errors.OfType<UserAlreadyRegisteredError>().FirstOrDefault();
  if (alreadyRegistered is not null)
      return Results.Conflict(new { error = alreadyRegistered.Message });

  // Wrong — fragile string matching
  if (result.Errors.Any(e => e.Message.Contains("already registered")))
      ...
  ```

## Configuration

- Use `IOptions<T>` for all typed configuration. Never read `IConfiguration` by string key directly inside application code.
