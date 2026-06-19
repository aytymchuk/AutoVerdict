# Test Configuration & Guidelines

## Structure

1. **Mirrored layout**: Each test project mirrors its source counterpart (e.g., `AutoVerdikt.Application.Tests` tests `AutoVerdikt.Application`). Folder structure inside a test project mirrors the source project.
2. **Shared config**: `tests/Directory.Build.props` marks all projects as `<IsTestProject>true</IsTestProject>` and pulls in xUnit, Shouldly, Coverlet, and the Test SDK automatically.
3. **Extra packages**: Add only project-specific packages (e.g., `Moq`) in the individual test `.csproj` — do not duplicate packages already provided by `Directory.Build.props`.

## Conventions

4. **Format**: Follow Arrange / Act / Assert with semantic test names: `<Method>_<Scenario>_<ExpectedOutcome>` (e.g., `Handle_AlreadyRegistered_ReturnsUserAlreadyRegisteredError`).
5. **Assertions**: All tests **MUST** use **Shouldly** (e.g., `result.IsSuccess.ShouldBeTrue()`). Do not use raw xUnit `Assert.*` calls.
6. **FluentValidation tests**: When testing `AbstractValidator<T>` subclasses, use the `FluentValidation.TestHelper` API — `validator.TestValidate(model)` returns a `TestValidationResult`; assert with `.ShouldHaveValidationErrorFor(x => x.Property).WithErrorMessage("...")` and `.ShouldNotHaveAnyValidationErrors()`. These purpose-built assertions take precedence over Shouldly for validation-specific checks.
7. **Error type assertions**: When a handler returns `Result.Fail(...)`, assert both the failure state and the concrete `Error` subtype: `result.Errors[0].ShouldBeOfType<UserAlreadyRegisteredError>()`. Do not rely on message strings alone.

## Integration Tests (`AutoVerdikt.WebApi.IntegrationTests`)

8. **Scope**: Verify positive (success) scenarios and key failure scenarios (e.g., duplicate registration, unauthorized access). Do **not** add validation tests (empty fields, invalid email format, etc.) — those belong in unit tests (`AutoVerdikt.WebApi.Tests` with FluentValidation.TestHelper, `AutoVerdikt.Application.Tests` for handlers).
9. **Edge cases**: Specific edge cases and input validation must be covered via **Unit Tests**, not integration tests.
10. **No infrastructure mocks**: Do not mock databases or blob storage. Run real dependencies in Docker via **Testcontainers**.
11. **Authentication**: May be mocked (test auth handler) for flexible user identity and roles without real Clerk JWTs.
12. **Layout**: One folder per feature under the integration test project; each feature has its own test base class (e.g., `Users/UsersTestBase.cs`) inheriting `BaseFixture`. Do not name feature classes `*Fixture` — that conflicts with AutoFixture’s `Fixture` type in C# 13.
13. **Isolation**: Tests must be independent and idempotent — use unique data per test (via `AutoFixture`) so tests never share identifiers or affect each other.
14. **Test data**: Use **AutoFixture** for generating test data — avoid hand-crafted dummy strings.
15. **Request payloads**: Serialize API request bodies with the same WebApi DTO types (e.g., `CreateResearchDto`, `UserRegistrationDto`) in `PostAsJsonAsync` / `PatchAsJsonAsync`. Do not use anonymous objects for request payloads. Exception: raw JSON strings are allowed when explicitly testing deserialization edge cases (e.g., unknown JSON properties rejected by the API).

## Architecture Tests

See `AutoVerdikt.Architecture.Tests/AGENTS.md` for rules specific to dependency-enforcement tests.
