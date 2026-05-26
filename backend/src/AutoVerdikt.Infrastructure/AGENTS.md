# Infrastructure Layer

Implements integrations with external services and APIs (CEPiK, VIN databases, third-party providers). Depends ONLY on Application.

## Rules

- **No forbidden dependencies**: Must NOT reference WebApi, Agents, or Store.
- **Application abstractions only**: Implement interfaces defined in Application. Never expose third-party SDK types (HTTP clients, response models) beyond this layer's boundary — wrap them in Application-defined types.
- **Failure signalling**: Return `Result`/`Result<T>` from service methods — do not propagate raw HTTP exceptions or SDK errors to callers. Map them to typed `Error` subclasses defined in Application.
- **Configuration**: Bind external service settings via `IOptions<T>` with `ValidateOnStart()` — fail fast if required keys (API base URLs, credentials) are missing at startup.
