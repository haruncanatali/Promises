using Promises.Application.Users.Queries.GetUserDetail;

namespace Promises.Application.Users.Queries.GetUsersList;

public class UserListVm
{
    public IList<UserDetailDto> Users { get; set; }

    public int Count { get; set; }
}