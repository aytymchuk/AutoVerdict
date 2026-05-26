# Architecture Tests

Uses **NetArchTest.Rules** to enforce the dependency rules declared in `src/AGENTS.md`. These tests are the automated gate — a failing architecture test means a layer boundary has been violated.

## What is tested

| Test | Rule enforced |
|---|---|
| `Domain_Should_Not_HaveDependencyOn_AnyOtherProject` | Domain has zero project dependencies |
| `Application_Should_Not_HaveDependencyOn_OtherProjects` | Application does not reference Infrastructure, Store, WebApi, or Agents |
| `Infrastructure_Should_Not_HaveDependencyOn_Store_WebApi_Or_Agents` | Infrastructure siblings isolation |
| `Store_Should_Not_HaveDependencyOn_Infrastructure_WebApi_Or_Agents` | Store siblings isolation |
| `Agents_Should_Not_HaveDependencyOn_Infrastructure_Store_Or_WebApi` | Agents siblings isolation |
| `WebApi_Should_Not_HaveDependencyOn_Agents` | Composition root does not depend on Agents |

## Adding new tests

- When a new layer or sibling constraint is introduced, add a corresponding `[Fact]` here.
- Use `Assembly.Load("<Namespace>")` to load the target assembly and `Types.InAssembly(...).ShouldNot().HaveDependencyOnAny(...)` to assert isolation.
- All assertions must use **Shouldly**: `result.IsSuccessful.ShouldBeTrue()`.
