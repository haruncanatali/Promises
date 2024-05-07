using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Friends.Commands.DeleteBlock
{
    public class DeleteBlockCommandValidator : AbstractValidator<DeleteBlockCommand>
    {
        public DeleteBlockCommandValidator()
        {
            RuleFor(c => c.ReceiverId).NotNull()
                .WithName(GlobalPropertyDisplayName.FriendReceiverId);
        }
    }
}
