using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Friends.Queries.GetBlockedFriends
{
    public class GetBlockedFriendsQuery : IRequest<BaseResponseModel<GetBlockedFriendsVm>>
    {
        public string? UserFullName { get; set; }
    }
}
