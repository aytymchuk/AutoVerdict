using FluentValidation;

namespace AutoVerdikt.WebApi.Endpoints.Users;

public sealed class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
{
    public UserRegistrationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");
    }
}
