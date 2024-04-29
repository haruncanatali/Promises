using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.EventPhotos.Commands.Update;

public class UpdateEventPhotoCommandValidator : AbstractValidator<UpdateEventPhotoCommand>
{
    public UpdateEventPhotoCommandValidator()
    {
        RuleFor(c => c.Id).NotNull()
            .WithName(GlobalPropertyDisplayName.EventPhotoId);
        RuleFor(c => c.AgreementId).NotNull()
            .WithName(GlobalPropertyDisplayName.EventAgreementId);
        RuleFor(c => c.Photo).NotNull()
            .WithName(GlobalPropertyDisplayName.EventPhotoStr);
    }
}