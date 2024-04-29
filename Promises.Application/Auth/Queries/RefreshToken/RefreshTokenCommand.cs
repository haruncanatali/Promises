using MediatR;
using Microsoft.AspNetCore.Identity;
using Promises.Application.Auth.Queries.Login.Dtos;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Domain.Identity;

namespace Promises.Application.Auth.Queries.RefreshToken;

public class RefreshTokenCommand: IRequest<BaseResponseModel<LoginDto>>
{
    public string RefreshToken { get; set; }

    public class Handler : IRequestHandler<RefreshTokenCommand, BaseResponseModel<LoginDto>>
    {
        private readonly TokenManager _tokenManager;
        private readonly UserManager<User> _userManager;

        public Handler(TokenManager tokenManager, UserManager<User> userManager)
        {
            _tokenManager = tokenManager;
            _userManager = userManager;
        }

        public async Task<BaseResponseModel<LoginDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            User? appUser = _userManager.Users.FirstOrDefault(x => x.RefreshToken == request.RefreshToken && x.RefreshTokenExpiredTime > DateTime.Now);
            if (appUser != null)
            {
                LoginDto loginViewModel = await _tokenManager.GenerateToken(appUser);
                return BaseResponseModel<LoginDto>.Success(data: loginViewModel,$"{appUser.UserName} kullanıcısı refreshToken isteği başarıyla işlendi.");
            }
            
            throw new BadRequestException($"RefreshToken isteği kullanıcı bulunamadığı için işlenemedi.");
        }
    }
}