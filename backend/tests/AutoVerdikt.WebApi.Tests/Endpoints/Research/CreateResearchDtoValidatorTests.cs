using AutoVerdikt.WebApi.Endpoints.Research;
using FluentValidation.TestHelper;

namespace AutoVerdikt.WebApi.Tests.Endpoints.Research;

public class CreateResearchDtoValidatorTests
{
    private readonly CreateResearchDtoValidator _validator = new();

    [Fact]
    public void Form_WithoutCar_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new CreateResearchDto("form", null, null));

        result.ShouldHaveValidationErrorFor(x => x.Car)
            .WithErrorMessage("Car data is required for the form input method.");
    }

    [Fact]
    public void Form_WithCar_ShouldNotHaveValidationErrors()
    {
        var car = new CreateCarDataDto(
            "Volkswagen",
            "Golf",
            2018,
            87200,
            42900,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        var result = _validator.TestValidate(new CreateResearchDto("form", car, null));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Text_WithoutText_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new CreateResearchDto("text", null, null));

        result.ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage("Text is required for the text input method.");
    }

    [Fact]
    public void Text_WithText_ShouldNotHaveValidationErrors()
    {
        var result = _validator.TestValidate(new CreateResearchDto("text", null, "VW Golf 2018, 87k km"));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Text_WithTextExceedingMaxLength_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new CreateResearchDto(
            "text",
            null,
            new string('a', CreateResearchDtoValidator.MaxTextLength + 1)));

        result.ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage($"Text must not exceed {CreateResearchDtoValidator.MaxTextLength} characters.");
    }
}
