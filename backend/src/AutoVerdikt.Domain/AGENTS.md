# Domain Layer

The innermost layer. Contains only pure business models — no dependencies on any other project or NuGet package.

## Rules

- **Zero dependencies**: The `.csproj` must have no `<ProjectReference>` and no `<PackageReference>`. Any violation breaks the architecture test.
- **Entity design**: Entities are `record` types with a `private` parameterless constructor and a `public static Create(...)` factory method. All settable properties use `init`.
- **IDs**: Use UUID v7 (`Guid.CreateVersion7(DateTimeOffset)`) — time-sortable and globally unique. Never use `Guid.NewGuid()` for entity IDs.
- **Time**: Accept `TimeProvider` in factory methods — never call `DateTime.UtcNow` or `DateTimeOffset.UtcNow` directly. Inject `TimeProvider.System` at the composition root; use a `FixedTimeProvider` in tests.
- **No infrastructure concerns**: No persistence attributes (BSON, EF), no serialization annotations, no HTTP or DI types.
