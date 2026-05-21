# Test Configuration & Guidelines

1. **Mirrored Structure**: Test projects are mirrored exactly from `src/` (e.g., `Application.Tests` tests `Application`).
2. **Centralized Config**: A separate `Directory.Build.props` in this folder marks `<IsTestProject>true</IsTestProject>` and `<IsPackable>false</IsPackable>`.
3. **Packages**: Global dependencies (xUnit, Coverlet, Test SDK, Shouldly) are strictly versioned in the root `Directory.Packages.props`.
4. **Architecture Tests**: Use `AutoVerdikt.Architecture.Tests` (`NetArchTest.Rules`) to enforce dependency rules across the solution.
   - **Crucial Rule**: `Agents` must depend on `Application`. `Application` must NEVER depend on `Agents` (Inversion of Control).
5. **Format**: Follow standard Arrange/Act/Assert xUnit patterns with semantic naming.
6. **Assertions**: All tests **MUST** use the `Shouldly` library for assertions (e.g., `result.ShouldBeTrue()`), rather than standard xUnit assertions.
7. **FluentValidation Tests**: When testing `AbstractValidator<T>` subclasses, use the `FluentValidation.TestHelper` API: `validator.TestValidate(model)` returns a `TestValidationResult`; assert with `.ShouldHaveValidationErrorFor(x => x.Property).WithErrorMessage("...")` and `.ShouldNotHaveAnyValidationErrors()`. These are the canonical assertions for validator unit tests and take precedence over Shouldly for validation-specific checks.
