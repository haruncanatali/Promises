using FluentValidation;
using Promises.Application.Common.Models;

namespace Promises.Application.Auth.Queries.HardPasswordChange;

public class HardPasswordChangeCommandValidator : AbstractValidator<HardPasswordChangeCommand>
{
    public HardPasswordChangeCommandValidator()
    {
        RuleFor(x => x.Password).NotEmpty().WithName(GlobalPropertyDisplayName.Password)
            .MinimumLength(8).WithMessage("Şifre uzunluğunuz en az 8 olmalıdır.")
            .Matches(@"[A-Z]+").WithMessage("Şifreniz en az bir büyük harf içermelidir.")
            .Matches(@"[a-z]+").WithMessage("Şifreniz en az bir küçük harf içermelidir.")
            .Matches(@"[0-9]+").WithMessage("Parolanız en az bir sayı içermelidir.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Şifreniz en az bir tane (!? *.) içermelidir.");
    }
}