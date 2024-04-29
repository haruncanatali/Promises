using MediatR;

namespace Promises.Application.Users.Queries.GetUsersList
{
    public class GetUserListQuery : IRequest<UserListVm>
    {
        public string? FullName { get; set; }
    }
}
