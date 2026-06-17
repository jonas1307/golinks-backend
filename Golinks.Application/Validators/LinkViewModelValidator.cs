using FluentValidation;
using Golinks.Application.ViewModel;

namespace Golinks.Application.Validators;

public class LinkViewModelValidator : AbstractValidator<LinkViewModel>
{
    public LinkViewModelValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .MaximumLength(2048)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var result)
                         && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
            .WithMessage("'Url' must be a valid HTTP or HTTPS URL.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
