using FluentValidation;

namespace AutoVerdikt.WebApi.Endpoints.Whitelist;

internal sealed class SubmitWaitlistRequestDtoValidator : AbstractValidator<SubmitWaitlistRequestDto>
{
    internal const int AboutMaxLength = 10000;

    public SubmitWaitlistRequestDtoValidator()
    {
        RuleFor(x => x.About)
            .MaximumLength(AboutMaxLength)
            .When(x => x.About is not null);
    }
}
