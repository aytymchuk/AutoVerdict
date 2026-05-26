# Agents Layer

Implements AI/LLM integrations (e.g., report generation, claim cross-checking). Depends ONLY on Application.

## Rules

- **No forbidden dependencies**: Must NOT reference WebApi, Infrastructure, or Store.
- **Application abstractions only**: Consume interfaces and commands defined in Application. Never call persistence or external-service code directly.
- **Prompt management**: Keep prompt templates as named constants or dedicated resource files — not inline string literals scattered across implementation code.
- **Failure signalling**: Return `Result`/`Result<T>` from AI service methods — do not propagate raw SDK or HTTP exceptions to callers.
