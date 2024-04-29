using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Persons.Commands.Create;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .WithName(GlobalPropertyDisplayName.PersonName);
        RuleFor(c => c.Name).NotEmpty()
            .WithName(GlobalPropertyDisplayName.PersonSurname);
        RuleFor(c => c.Age).NotEmpty()
            .WithName(GlobalPropertyDisplayName.PersonAge);

    }
}