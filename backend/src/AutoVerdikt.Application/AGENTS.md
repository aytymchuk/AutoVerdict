# Application Layer

The core business-logic layer. All outer layers depend on it; it depends only on Domain.

## Rules

- **No outbound project dependencies**: Must NOT reference Infrastructure, Store, WebApi, or Agents. Any violation breaks the architecture test.
- **Define abstractions**: Declare repository and service interfaces here (e.g., `IUserRepository`). Implementations live in the outer layers.
- **Commands & Queries**: Use `Mediator.Abstractions` (`IRequest<T>`, `IRequestHandler<TRequest, TResponse>`). Each command/query and its handler live together in an operation-specific sub-folder under `<Feature>/` (e.g., `Users/Register/`, `Whitelist/Admin/Add/`). Name the folder after the operation verb (`Add`, `Remove`, `Approve`, `GetCurrent`, etc.) — not after the full type name.
- **Cross-feature code**: Shared types, utilities, and services used by multiple features live in top-level folders — not inside `<Feature>/`. Examples: `Abstractions/`, `Behaviors/`, `Common/`, `Errors/`, `FeatureFlags/`, `Identity/`. Feature folders (`Users/`, `Whitelist/`, etc.) contain only feature-specific commands, queries, handlers, repository interfaces, services, and errors.

## Logging & Observability

- **Pipeline logging and tracing**: `Behaviors/LoggingPipelineBehavior.cs` logs and traces every command and query (start, success, failure, exception) via `PipelineLog` and an OTEL `ActivitySource`. Do not inject `ILogger` into handlers or add per-handler start/complete logs for routine execution.
- **Handler-specific logs (edge cases)**: When a handler needs an extra log for a specific scenario, add a `[LoggerMessage]` method to `Behaviors/Logging/PipelineLog.cs` (next `EventId`, `internal static partial void`) and call it from the handler with the injected `ILogger<THandler>`.

```csharp
[LoggerMessage(EventId = 1005, Level = LogLevel.Information,
    Message = "User {UserId} upgraded to premium")]
internal static partial void UserUpgradedToPremium(ILogger logger, string userId);
```

## Error Handling

Return `Result` or `Result<T>` (FluentResults) from every operation that can fail in an expected way. Never throw or declare custom `Exception` subclasses for business failures.

### Error class convention

```csharp
public sealed class UserAlreadyRegisteredError() : Error("User is already registered.");
```

- Inherits `FluentResults.Error`, marked `sealed`.
- Message is set via the base constructor — no separate `Message` property.

### Error class placement

| Scope | Folder | Example |
|---|---|---|
| Feature-specific | `<Feature>/Errors/` | `Users/Errors/UserAlreadyRegisteredError.cs` |
| Cross-feature (shared) | `Errors/` | `Errors/UnauthorizedError.cs` |
