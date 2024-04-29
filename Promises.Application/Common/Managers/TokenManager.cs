using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Promises.Application.Auth.Queries.Login.Dtos;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Identity;

namespace Promises.Application.Common.Managers;

public class TokenManager
{
    private readonly TokenSetting _tokenSetting;
    private readonly UserManager<User> _userManager;
    private readonly IApplicationContext _context;

    public TokenManager(IOptions<TokenSetting> tokenSetting, UserManager<User> userManager, IApplicationContext context)
    {
        _tokenSetting = tokenSetting.Value;
        _userManager = userManager;
        _context = context;
    }

    public async Task<LoginDto> GenerateToken(User appUser)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(ClaimTypes.Name, appUser.UserName),
        };
        string responseRole = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();
        if (!string.IsNullOrEmpty(responseRole))
        {
            claims.Add(new Claim(ClaimTypes.Role, responseRole));
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSetting.Key));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        DateTime tokenExpire = DateTime.Now.AddHours(_tokenSetting.TokenValidityTime);

        JwtSecurityToken token = new JwtSecurityToken(
            _tokenSetting.Issuer,
            _tokenSetting.Audience,
            claims,
            expires: tokenExpire,
            signingCredentials: credentials
        );

        LoginDto summary = new LoginDto()
        {
            Email = appUser.Email,
            FirstName = appUser.FirstName,
            LastName = appUser.Surname,
            Role = responseRole,
        };
        summary.Token = new JwtSecurityTokenHandler().WriteToken(token);

        DateTime refreshTokenExpire = DateTime.Now.AddMinutes(_tokenSetting.RefreshTokenValidityTime);
        var refreshToken = CreateRefreshToken();
        summary.RefreshToken = refreshToken;
        summary.RefreshTokenExpireTime = refreshTokenExpire;
        summary.TokenExpireTime = tokenExpire;
        await _userManager.UpdateAsync(appUser);
        return summary;
    }


    private string CreateRefreshToken()
    {
        byte[] bytes = new byte[32];
        using (RandomNumberGenerator random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
            return Guid.NewGuid().ToString("N") + string.Concat(bytes.Select(x => x.ToString("x2")));
        }
    }
}