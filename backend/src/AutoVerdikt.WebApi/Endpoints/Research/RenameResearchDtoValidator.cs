using FluentValidation;

namespace AutoVerdikt.WebApi.Endpoints.Research;

public sealed class RenameResearchDtoValidator : AbstractValidator<RenameResearchDto>
{
    public RenameResearchDtoValidator()
    {
        RuleFor(x => x.NewName)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must be at most 200 characters.");
    }
}
