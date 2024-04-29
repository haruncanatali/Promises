using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.EventPhotos.Commands.Create;

public class CreateEventPhotoCommandValidator : AbstractValidator<CreateEventPhotoCommand>
{
    public CreateEventPhotoCommandValidator()
    {
        RuleFor(c => c.Photo).NotNull()
            .WithName(GlobalPropertyDisplayName.EventPhotoStr);
        RuleFor(c => c.AgreementId).NotNull()
            .WithName(GlobalPropertyDisplayName.EventAgreementId);
    }
}