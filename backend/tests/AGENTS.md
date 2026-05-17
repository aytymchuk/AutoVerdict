# Test Configuration & Guidelines

1. **Mirrored Structure**: Test projects are mirrored exactly from `src/` (e.g., `Application.Tests` tests `Application`).
2. **Centralized Config**: A separate `Directory.Build.props` in this folder marks `<IsTestProject>true</IsTestProject>` and `<IsPackable>false</IsPackable>`.
3. **Packages**: Global dependencies (xUnit, Coverlet, Test SDK) are strictly versioned in the root `Directory.Packages.props`.
4. **Architecture Tests**: Use `AutoVerdikt.Architecture.Tests` (`NetArchTest.Rules`) to enforce dependency rules across the solution.
5. **Format**: Follow standard Arrange/Act/Assert xUnit patterns with semantic naming.
6. **Assertions**: All tests **MUST** use the `Shouldly` library for assertions (e.g., `result.ShouldBeTrue()`), rather than standard xUnit assertions.
