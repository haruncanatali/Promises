using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Promises.Application.Friends.Queries.Dtos;

namespace Promises.Application.Friends.Queries.GetNotFriends
{
    public class GetNotFriendsVm
    {
        public List<NotFriendDto> NotFriends { get; set; }
        public long Count { get; set; }
    }
}
