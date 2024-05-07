using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Promises.Application.Common.Models;
using Promises.Application.Friends.Queries.Dtos;

namespace Promises.Application.Friends.Queries.GetNotFriends
{
    public class GetNotFriendsQuery : IRequest<BaseResponseModel<GetNotFriendsVm>>
    {
        public string? UserFullName { get; set; }
    }
}
