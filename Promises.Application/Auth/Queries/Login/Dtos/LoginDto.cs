namespace Promises.Application.Auth.Queries.Login.Dtos;

public class LoginDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
    public DateTime TokenExpireTime { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}