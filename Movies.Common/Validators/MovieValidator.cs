using System;
using FluentValidation;
using Movies.Common.Dto;

namespace Movies.Common.Validators
{
    public class MovieValidator : AbstractValidator<MovieDto>
    {
        public MovieValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Director)
                .NotEmpty().WithMessage("Director is required");
            RuleFor(x => x.Released)
                .LessThan(DateTime.Today).WithMessage("Released can't be greater than actual day");
            RuleFor(x => x.UserId)
                .NotNull().NotEmpty().WithMessage("User is required");
        }
    }
}
