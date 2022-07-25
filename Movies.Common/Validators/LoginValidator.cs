using FluentValidation;
using Movies.Common.Dto;

namespace Movies.Common.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(10).WithMessage("Password should be greater than 10 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least 1 upper case letter")
                .Matches("[a-z]").WithMessage("Password must contain at least 1 lower case letter")
                .Matches(@"[!@#?]").WithMessage("Password must contain at least 1 special character");
        }
    }
}
