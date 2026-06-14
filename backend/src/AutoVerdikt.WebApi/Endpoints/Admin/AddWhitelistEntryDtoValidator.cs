using FluentValidation;

namespace AutoVerdikt.WebApi.Endpoints.Admin;

internal sealed class AddWhitelistEntryDtoValidator : AbstractValidator<AddWhitelistEntryDto>
{
    public AddWhitelistEntryDtoValidator()
    {
        RuleFor(x => x.AuthId).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
