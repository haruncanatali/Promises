using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Persons.Commands.Delete;

public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(c => c.Id).NotNull()
            .WithName(GlobalPropertyDisplayName.PersonId);
    }
}