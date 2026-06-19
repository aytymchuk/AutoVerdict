# Domain Layer

The innermost layer. Contains only pure business models — no dependencies on any other project or NuGet package.

## Rules

- **Zero dependencies**: The `.csproj` must have no `<ProjectReference>` and no `<PackageReference>`. Any violation breaks the architecture test.
- **Entity design**: Entities are `record` types with `required init` properties and a `public static Create(...)` factory method. Use `required` for all non-nullable, always-present fields; leave nullable/optional fields as plain `init`. The Store layer hydrates records from persistence using object initializer syntax directly.
- **IDs**: Use UUID v7 (`Guid.CreateVersion7(DateTimeOffset)`) — time-sortable and globally unique. Never use `Guid.NewGuid()` for entity IDs.
- **Time**: Accept `TimeProvider` in factory methods — never call `DateTime.UtcNow` or `DateTimeOffset.UtcNow` directly. Inject `TimeProvider.System` at the composition root; use a `FixedTimeProvider` in tests.
- **No infrastructure concerns**: No persistence attributes (BSON, EF), no serialization annotations, no HTTP or DI types.
- **Domain mutation methods**: Domain records expose focused `with`-based methods for state transitions (e.g. `Approve`, `Reject`, `ChangeWhitelistStatus`). Use these in application/command handlers instead of rebuilding the full record.
