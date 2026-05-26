# Store Layer

Implements MongoDB persistence. Depends ONLY on Application.

## Rules

- **No forbidden dependencies**: Must NOT reference WebApi, Infrastructure, or Agents.
- **Internal documents**: Map domain models to `internal sealed class *Document` types decorated with BSON attributes. Never expose document types outside this project.
- **Implement Application interfaces**: Every `I*Repository` defined in Application must be implemented here and registered in `StoreServiceExtensions`.
- **Failure signalling**: Return `Result`/`Result<T>` from repository methods — do not throw custom exceptions. Catch infrastructure exceptions (e.g., `MongoWriteException` code 11000 for duplicate keys) and convert them to typed `Error` returns from Application.
- **Startup validation**: Bind configuration via `AddOptions<T>().Bind(...).Validate(...).ValidateOnStart()` — fail fast on missing or invalid config rather than deferring the error to the first DB operation.
- **Indexes**: Create unique and other indexes in `MongoDbInitializer` (an `IHostedService` registered at startup) rather than creating them on demand.
