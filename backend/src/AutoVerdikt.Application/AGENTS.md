# Application Layer

The core business-logic layer. All outer layers depend on it; it depends only on Domain.

## Rules

- **No outbound project dependencies**: Must NOT reference Infrastructure, Store, WebApi, or Agents. Any violation breaks the architecture test.
- **Define abstractions**: Declare repository and service interfaces here (e.g., `IUserRepository`). Implementations live in the outer layers.
- **Commands & Queries**: Use `Mediator.Abstractions` (`IRequest<T>`, `IRequestHandler<TRequest, TResponse>`). Commands and their handlers live together in `<Feature>/` sub-folders (e.g., `Users/Register/`).

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
