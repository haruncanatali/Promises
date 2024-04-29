using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Persons.Commands.Update;

public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(c => c.Id).NotNull()
            .WithName(GlobalPropertyDisplayName.PersonId);
        RuleFor(c => c.Name).NotEmpty()
            .WithName(GlobalPropertyDisplayName.PersonName);
        RuleFor(c => c.Name).NotEmpty()
            .WithName(GlobalPropertyDisplayName.PersonSurname);
        RuleFor(c => c.Age).NotEmpty()
            .WithName(GlobalPropertyDisplayName.PersonAge);
    }
}