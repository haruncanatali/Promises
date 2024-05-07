using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Friends.Commands.Delete
{
    public class DeleteFriendCommandValidator : AbstractValidator<DeleteFriendCommand>
    {
        public DeleteFriendCommandValidator()
        {
            RuleFor(c => c.FriendId).NotNull()
                .WithName(GlobalPropertyDisplayName.FriendId);
            RuleFor(c => c.UserId).NotNull()
                .WithName(GlobalPropertyDisplayName.FriendUserId);
        }
    }
}
