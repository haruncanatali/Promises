using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Agreements.Commands.Delete;

public class DeleteAgreementCommandValidator : AbstractValidator<DeleteAgreementCommand>
{
    public DeleteAgreementCommandValidator()
    {
        RuleFor(c => c.Id).NotNull()
            .WithName(GlobalPropertyDisplayName.AgreementId);
    }
}