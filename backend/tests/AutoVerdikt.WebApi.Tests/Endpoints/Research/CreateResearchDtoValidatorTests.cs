using AutoVerdikt.WebApi.Endpoints.Research;
using FluentValidation.TestHelper;

namespace AutoVerdikt.WebApi.Tests.Endpoints.Research;

public class CreateResearchDtoValidatorTests
{
    private readonly CreateResearchDtoValidator _validator = new();

    [Fact]
    public void Form_WithoutCar_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new CreateResearchDto("form", null));

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

        var result = _validator.TestValidate(new CreateResearchDto("form", car));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Text_WithoutCar_ShouldNotHaveValidationErrors()
    {
        var result = _validator.TestValidate(new CreateResearchDto("text", null));

        result.ShouldNotHaveAnyValidationErrors();
    }
}
