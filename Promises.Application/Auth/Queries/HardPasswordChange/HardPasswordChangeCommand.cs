using MediatR;
using Microsoft.AspNetCore.Identity;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Identity;
namespace Promises.Application.Auth.Queries.HardPasswordChange;

public class HardPasswordChangeCommand : IRequest<BaseResponseModel<User>>
{
    public string Password { get; set; }

    public class Handler : IRequestHandler<HardPasswordChangeCommand, BaseResponseModel<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public Handler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseModel<User>> Handle(HardPasswordChangeCommand request, CancellationToken cancellationToken)
        {
            User? appUser = _userManager.Users.FirstOrDefault(x => x.Id == _currentUserService.UserId);
            if (appUser != null)
            {
                var removeResult = await _userManager.RemovePasswordAsync(appUser);
                if (removeResult.Succeeded)
                {
                    var addResult = await _userManager.AddPasswordAsync(appUser, request.Password);
                    if (addResult.Succeeded)
                    {
                        return BaseResponseModel<User>.Success(appUser,$"Şifre değiştirildi. Güncelleme yapılan kullanıcı : {appUser.UserName}");
                    }
                    else
                    {
                        throw new BadRequestException($"Şifre değiştirilemedi. Hata meydana geldi. Güncelleme yapılamıyan kullanıcı : {appUser.UserName}");
                    }
                }
                else
                {
                    throw new BadRequestException("Şifre Silinemedi!");
                }
            }
            
            throw new NotFoundException(nameof(User), $"(HPCC-2) Şifre değiştirilemedi. Kullanıcı bulunamadı. Güncellenmek istenen kullanıcı ID:{_currentUserService.UserId}");
        }
    }
}