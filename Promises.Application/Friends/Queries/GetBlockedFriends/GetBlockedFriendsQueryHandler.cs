using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Friends.Queries.Dtos;
using Promises.Domain.Entities;
using Promises.Domain.Identity;

namespace Promises.Application.Friends.Queries.GetBlockedFriends
{
    public class GetBlockedFriendsQueryHandler : IRequestHandler<GetBlockedFriendsQuery, BaseResponseModel<GetBlockedFriendsVm>>
    {
        private readonly IApplicationContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetBlockedFriendsQueryHandler(IApplicationContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseModel<GetBlockedFriendsVm>> Handle(GetBlockedFriendsQuery request, CancellationToken cancellationToken)
        {
            List<long> blockedFriendsIds = await _context.BlockedFriends
                .Where(c => c.SenderId == _currentUserService.UserId)
                .Select(c => c.ReceiverId)
                .ToListAsync(cancellationToken);

            List<NotFriendDto> users = await _context.Users
                .Where(c => blockedFriendsIds.Contains(c.Id))
                .Select(c => new NotFriendDto
                {
                    UserId = c.Id,
                    UserFullName = c.FullName
                })
                .ToListAsync(cancellationToken);

            return BaseResponseModel<GetBlockedFriendsVm>.Success(new GetBlockedFriendsVm
            {
                BlockedUsers = users,
                Count = users.Count
            }, "Engellenenler listesi başarıyla çekildi.");
        }
    }
}
