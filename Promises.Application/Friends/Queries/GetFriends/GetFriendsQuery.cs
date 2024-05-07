using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Friends.Queries.GetFriends
{
    public class GetFriendsQuery : IRequest<BaseResponseModel<GetFriendsVm>>
    {
        public string? FullName { get; set; }
        public bool Approved { get; set; }
    }
}
