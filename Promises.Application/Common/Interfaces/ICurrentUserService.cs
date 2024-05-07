namespace Promises.Application.Common.Interfaces;

public interface ICurrentUserService
{
    long UserId { get; }
    string FullName { get; }
    bool IsAuthenticated { get; }
}