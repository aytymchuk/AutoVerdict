using FluentValidation;

namespace AutoVerdikt.WebApi.Endpoints.Research;

public sealed class CreateResearchDtoValidator : AbstractValidator<CreateResearchDto>
{
    public CreateResearchDtoValidator()
    {
        RuleFor(x => x.InputMethod)
            .NotEmpty()
            .WithMessage("Input method is required.")
            .Must(method => method is "form" or "text")
            .WithMessage("Input method must be a supported value.");

        When(x => x.InputMethod == "form", () =>
        {
            RuleFor(x => x.Car)
                .NotNull()
                .WithMessage("Car data is required for the form input method.");
        });

        When(x => x.InputMethod == "text", () =>
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Text is required for the text input method.");
        });
    }
}
