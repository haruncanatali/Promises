using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Promises.Application.Friends.Queries.Dtos;

namespace Promises.Application.Friends.Queries.GetFriends
{
    public class GetFriendsVm
    {
        public List<FriendDto> Friends { get; set; }
        public long Count { get; set; }
    }
}
