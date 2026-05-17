# Backend Clean Architecture Restrictions

- **Application**: The core. Must have **NO** dependencies on other projects.
- **Agents**: Implements AI/LLM integrations. Depends **ONLY** on Application.
- **Infrastructure**: Implements external services/APIs. Depends **ONLY** on Application.
- **Store**: Implements database and persistence. Depends **ONLY** on Application.
- **WebApi**: The composition root. Depends on all layers to wire up Dependency Injection.

*Crucial: `Agents`, `Infrastructure`, and `Store` are strictly siblings and must NEVER depend on each other.*
- **Security**: Authentication is required by default for all endpoints via a global fallback policy using Clerk JWTs.
- **Configuration**: Always use strongly-typed configuration classes (`IOptions` pattern or `Get<T>`) rather than reading values by key directly from `IConfiguration`.
- **Magic Strings**: Avoid using magic strings (values) directly in the code, especially if they are vendor-specific. Use constants or configurations instead.
