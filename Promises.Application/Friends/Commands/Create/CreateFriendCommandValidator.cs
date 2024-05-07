using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Friends.Commands.Create;

public class CreateFriendCommandValidator : AbstractValidator<CreateFriendCommand>
{
    public CreateFriendCommandValidator()
    {
        RuleFor(c => c.ReceiverId).NotNull()
            .WithName(GlobalPropertyDisplayName.FriendReceiverId);
    }
}