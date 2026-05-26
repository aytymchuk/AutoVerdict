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

## Architecture Tests

See `AutoVerdikt.Architecture.Tests/AGENTS.md` for rules specific to dependency-enforcement tests.
