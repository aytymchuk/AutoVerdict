using AutoVerdikt.WebApi.Endpoints.Users;
using FluentValidation.TestHelper;

namespace AutoVerdikt.WebApi.Tests.Endpoints.Users;

public class UserRegistrationDtoValidatorTests
{
    private readonly UserRegistrationDtoValidator _validator = new();

    [Fact]
    public void Name_Empty_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new UserRegistrationDto(string.Empty, "alice@example.com"));

        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void Name_Whitespace_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new UserRegistrationDto("   ", "alice@example.com"));

        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void Email_Empty_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new UserRegistrationDto("Alice", string.Empty));

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Email_InvalidFormat_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new UserRegistrationDto("Alice", "not-an-email"));

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("A valid email address is required.");
    }

    [Fact]
    public void Email_MissingAtSign_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new UserRegistrationDto("Alice", "aliceexample.com"));

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("A valid email address is required.");
    }

    [Fact]
    public void ValidInput_ShouldNotHaveAnyValidationErrors()
    {
        var result = _validator.TestValidate(new UserRegistrationDto("Alice", "alice@example.com"));

        result.ShouldNotHaveAnyValidationErrors();
    }
}
