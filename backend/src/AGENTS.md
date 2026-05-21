# Backend Clean Architecture Restrictions

- **Application**: The core. Must have **NO** dependencies on other projects.
- **Agents**: Implements AI/LLM integrations. Depends **ONLY** on Application.
- **Infrastructure**: Implements external services/APIs. Depends **ONLY** on Application.
- **Store**: Implements database and persistence. Depends **ONLY** on Application.
- **WebApi**: The composition root. Depends on all layers to wire up Dependency Injection.

*Crucial: `Agents`, `Infrastructure`, and `Store` are strictly siblings and must NEVER depend on each other.*
- **Security**: Authentication is required by default for all endpoints via a global fallback policy using Clerk JWTs.
- **Configuration**: Always use strongly-typed configuration classes (`IOptions` pattern or `Get<T>`) rather than reading values by key directly from `IConfiguration`.
- **Magic Strings**: Avoid using magic strings (values) directly in the code, especially if they are vendor-specific. Use constants or configurations instead. Endpoint routes, names, and fixed response values must be defined as constants in a dedicated class under `Endpoints/` (e.g., `Endpoints/HealthEndpoint.cs`).
- **HTTP Request Validation**: All incoming HTTP request DTOs must be validated using **FluentValidation**. Validators live in the same `Endpoints/<Feature>/` folder as the DTO they validate, and are named `<Dto>Validator`. Wire validation into the HTTP pipeline via `SharpGrip.FluentValidation.AutoValidation.Endpoints` — add `.AddFluentValidationAutoValidation()` on the `RouteHandlerBuilder` for each endpoint. Register all validators in `Program.cs` with `AddValidatorsFromAssemblyContaining<Program>()` and `AddFluentValidationAutoValidation()`. Do **not** perform manual validation inside endpoint handlers.
- **Error Handling — prefer `Error` over exceptions**: Use **FluentResults** (`Result`, `Result<T>`) as the return type for any operation that can fail in an expected way (business rules, duplicate records, not-found, etc.). Do **not** throw custom exception classes for these cases — instead return `Result.Fail(new SomeError())`. Custom exception classes are prohibited; use typed `Error` subclasses instead.
  - **Error class placement**: Create a dedicated `Errors/` folder at the appropriate scope:
    - **Feature-level** (`Application/<Feature>/Errors/`) — for errors that belong to a single feature (e.g., `Users/Errors/UserAlreadyRegisteredError.cs`).
    - **Global** (`Application/Errors/`) — for cross-cutting errors shared across multiple features.
  - **Error class convention**: Each error class inherits from `FluentResults.Error`, is `sealed`, and sets its message via the base constructor: `public sealed class UserAlreadyRegisteredError() : Error("User is already registered.");`
  - **Allowed exceptions**: Reserve exceptions for truly unexpected program errors — infrastructure misconfiguration (startup guards), broken invariants, or programmer errors. Do **not** wrap these in `Result`.
