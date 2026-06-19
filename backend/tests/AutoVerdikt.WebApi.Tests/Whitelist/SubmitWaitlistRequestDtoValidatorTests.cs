using AutoVerdikt.WebApi.Endpoints.Whitelist;
using FluentValidation.TestHelper;

namespace AutoVerdikt.WebApi.Tests.Whitelist;

public sealed class SubmitWaitlistRequestDtoValidatorTests
{
    private readonly SubmitWaitlistRequestDtoValidator _validator = new();

    [Fact]
    public void About_WithinMaxLength_NoValidationError()
    {
        var dto = new SubmitWaitlistRequestDto(new string('a', SubmitWaitlistRequestDtoValidator.AboutMaxLength));
        _validator.TestValidate(dto).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void About_ExceedsMaxLength_HasValidationError()
    {
        var dto = new SubmitWaitlistRequestDto(new string('a', SubmitWaitlistRequestDtoValidator.AboutMaxLength + 1));
        _validator.TestValidate(dto)
            .ShouldHaveValidationErrorFor(x => x.About);
    }
}
