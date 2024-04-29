using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Roles.Commands.AddToRole;

public class AddToRoleCommandValidator : AbstractValidator<AddToRoleCommand>
{
    public AddToRoleCommandValidator()
    {
        RuleFor(c => c.RoleId).NotNull()
            .WithName(GlobalPropertyDisplayName.RoleId);
        RuleFor(c => c.UserId).NotNull()
            .WithName(GlobalPropertyDisplayName.UserId);
    }
}