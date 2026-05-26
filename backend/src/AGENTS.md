# Clean Architecture — Layer Overview

## Dependency Rules

| Layer | Depends On |
|---|---|
| **Domain** | _(nothing)_ — innermost layer |
| **Application** | Domain only |
| **Agents** | Application only |
| **Infrastructure** | Application only |
| **Store** | Application only |
| **WebApi** | Application · Infrastructure · Store (composition root) |

`Agents`, `Infrastructure`, and `Store` are **strictly siblings** — they must NEVER depend on each other.

## Cross-Cutting Policies

- **Error handling**: Use `FluentResults` (`Result`, `Result<T>`) for expected failures. Never create custom `Exception` subclasses for business errors — use typed `Error` subclasses instead. See `AutoVerdikt.Application/AGENTS.md` for placement and naming rules.
- **Configuration**: Always bind config via strongly-typed classes using the `IOptions<T>` pattern or `configuration.Get<T>()`. Never read `IConfiguration` by string key directly.
- **No magic strings**: Vendor-specific values, endpoint routes, and fixed response values must be defined as constants — never inlined as literals.
