using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithName(GlobalPropertyDisplayName.FirstName);
            RuleFor(x => x.Surname).NotEmpty().WithName(GlobalPropertyDisplayName.LastName);
            RuleFor(x => x.Email).EmailAddress().WithName(GlobalPropertyDisplayName.Email);
            RuleFor(c => c.Gender).NotNull().WithMessage(GlobalPropertyDisplayName.Gender);
            RuleFor(c => c.ZodiacSign).NotNull().WithMessage(GlobalPropertyDisplayName.ZodiacSign);
            RuleFor(c => c.Birthdate).NotNull().WithMessage(GlobalPropertyDisplayName.Birthdate);
        }
    }
}