using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Agreements.Commands.Update;

public class UpdateAgreementCommandValidator : AbstractValidator<UpdateAgreementCommand>
{
    public UpdateAgreementCommandValidator()
    {
        RuleFor(c => c.Id).NotNull()
            .WithName(GlobalPropertyDisplayName.AgreementId);
        RuleFor(c => c.Title).NotEmpty()
            .WithName(GlobalPropertyDisplayName.AgreementTitle);
        RuleFor(c => c.Description).NotEmpty()
            .WithName(GlobalPropertyDisplayName.AgreementDescription);
        RuleFor(c => c.PriorityLevel).NotNull()
            .WithName(GlobalPropertyDisplayName.AgreementPriorityLevel);
        RuleFor(c => c.Date).NotNull()
            .WithName(GlobalPropertyDisplayName.AgreementDate);
        RuleFor(c => c.HasNotification).NotNull()
            .WithName(GlobalPropertyDisplayName.AgreementHasNotification);
        RuleFor(c => c.HasMailNotification).NotNull()
            .WithName(GlobalPropertyDisplayName.AgreementHasMailNotification);
        RuleFor(c => c.NotificationFrequency).NotNull()
            .WithName(GlobalPropertyDisplayName.AgreementNotificationFrequency);
        RuleFor(c => c.UserId).NotNull()
            .WithName(GlobalPropertyDisplayName.PersonId);
    }
}