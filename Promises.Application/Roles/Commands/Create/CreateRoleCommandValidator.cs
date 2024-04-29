using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Roles.Commands.Create;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .WithName(GlobalPropertyDisplayName.RoleName);
    }
}