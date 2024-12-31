using FluentValidation;
using N_Tier.Application.DataTransferObjects.Authentication;

namespace N_Tier.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email may not be empty")
            .EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password may not be empty")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }

}
