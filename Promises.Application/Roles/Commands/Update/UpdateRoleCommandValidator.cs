using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Roles.Commands.Update;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .WithName(GlobalPropertyDisplayName.RoleName);
        RuleFor(c => c.Id).NotNull()
            .WithName(GlobalPropertyDisplayName.RoleId);
    }
}