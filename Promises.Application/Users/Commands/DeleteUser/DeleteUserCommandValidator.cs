using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName(GlobalPropertyDisplayName.UpdateId);
        }
    }
}
