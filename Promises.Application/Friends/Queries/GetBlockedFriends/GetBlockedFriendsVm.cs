using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Promises.Application.Friends.Queries.Dtos;

namespace Promises.Application.Friends.Queries.GetBlockedFriends
{
    public class GetBlockedFriendsVm
    {
        public List<NotFriendDto> BlockedUsers { get; set; }
        public long Count { get; set; }
    }
}
