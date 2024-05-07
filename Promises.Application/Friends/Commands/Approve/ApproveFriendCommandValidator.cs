using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Friends.Commands.Approve;

public class ApproveFriendCommandValidator : AbstractValidator<ApproveFriendCommand>
{
    public ApproveFriendCommandValidator()
    {
        RuleFor(c => c.UserId).NotNull()
            .WithName(GlobalPropertyDisplayName.FriendUserId);
        RuleFor(c => c.Approve).NotNull()
            .WithName(GlobalPropertyDisplayName.FriendApprovedState);
        RuleFor(c => c.FriendId).NotNull()
            .WithName(GlobalPropertyDisplayName.FriendId);
    }
}