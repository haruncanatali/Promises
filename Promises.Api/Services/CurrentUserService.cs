using System.Security.Claims;
using Promises.Application.Common.Interfaces;

namespace Promises.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        string UserIdStr = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        UserId = Convert.ToInt64(UserIdStr);
        IsAuthenticated = UserId != null;
    }

    public long UserId { get; }
    public bool IsAuthenticated { get; }
}