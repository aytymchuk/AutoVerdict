using AutoVerdikt.WebApi.Endpoints.Research;
using FluentValidation.TestHelper;

namespace AutoVerdikt.WebApi.Tests.Endpoints.Research;

public class RenameResearchDtoValidatorTests
{
    private readonly RenameResearchDtoValidator _validator = new();

    [Fact]
    public void NewName_Empty_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new RenameResearchDto(string.Empty));

        result.ShouldHaveValidationErrorFor(x => x.NewName)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void NewName_TooLong_ShouldHaveValidationError()
    {
        var result = _validator.TestValidate(new RenameResearchDto(new string('a', 201)));

        result.ShouldHaveValidationErrorFor(x => x.NewName)
            .WithErrorMessage("Name must be at most 200 characters.");
    }
}
