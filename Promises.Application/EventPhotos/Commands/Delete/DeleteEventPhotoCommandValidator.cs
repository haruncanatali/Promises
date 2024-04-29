using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.EventPhotos.Commands.Delete;

public class DeleteEventPhotoCommandValidator : AbstractValidator<DeleteEventPhotoCommand>
{
    public DeleteEventPhotoCommandValidator()
    {
        RuleFor(c => c.Id).NotNull()
            .WithName(GlobalPropertyDisplayName.EventPhotoId);
    }
}