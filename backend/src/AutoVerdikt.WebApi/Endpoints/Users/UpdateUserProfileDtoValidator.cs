using FluentValidation;

namespace AutoVerdikt.WebApi.Endpoints.Users;

public sealed class UpdateUserProfileDtoValidator : AbstractValidator<UpdateUserProfileDto>
{
    private static readonly string[] AllowedLanguages = ["en", "pl", "uk"];
    private static readonly string[] AllowedCurrencies = ["PLN", "UAH", "EUR", "USD"];

    public UpdateUserProfileDtoValidator()
    {
        RuleFor(x => x.Language)
            .Must(v => string.IsNullOrWhiteSpace(v) || AllowedLanguages.Contains(v))
            .WithMessage("Language must be one of: en, pl, uk, or null.");

        RuleFor(x => x.DefaultCurrency)
            .Must(v => string.IsNullOrWhiteSpace(v) || AllowedCurrencies.Contains(v))
            .WithMessage("DefaultCurrency must be one of: PLN, UAH, EUR, USD, or null.");
    }
}
