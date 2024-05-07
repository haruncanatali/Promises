using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Friends.Queries.Dtos;

namespace Promises.Application.Friends.Queries.GetNotFriends
{
    public class GetNotFriendsQueryHandler : IRequestHandler<GetNotFriendsQuery, BaseResponseModel<GetNotFriendsVm>>
    {
        private readonly IApplicationContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetNotFriendsQueryHandler(IApplicationContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseModel<GetNotFriendsVm>> Handle(GetNotFriendsQuery request, CancellationToken cancellationToken)
        {
            List<NotFriendDto> notFriendsList = new List<NotFriendDto>();

            List<long> friendsIds = await _context.Friends
                .Where(c => (c.ReceiverId == _currentUserService.UserId || c.SenderId == _currentUserService.UserId))
                .Select(c => c.ReceiverId == _currentUserService.UserId ? c.SenderId : c.ReceiverId)
                .ToListAsync(cancellationToken);

            var blockedIds = await _context.BlockedFriends
                .Where(c => (c.ReceiverId == _currentUserService.UserId || c.SenderId == _currentUserService.UserId))
                .Select(c => c.ReceiverId == _currentUserService.UserId ? c.SenderId : c.ReceiverId)
                .ToListAsync(cancellationToken);

            var notFriends = await _context.Users
                .Where(c=>friendsIds.Contains(c.Id).Equals(false) &&
                          (blockedIds.Contains(c.Id).Equals(false)))
                .Select(c=>new
                {
                    UserId = c.Id,
                    UserFullName = c.FullName
                })
                .ToListAsync(cancellationToken);

            notFriends.ForEach(c =>
            {
                notFriendsList.Add(new NotFriendDto
                {
                    UserId = c.UserId,
                    UserFullName = c.UserFullName
                });
            });

            return BaseResponseModel<GetNotFriendsVm>.Success(new GetNotFriendsVm
            {
                NotFriends = notFriendsList,
                Count = notFriendsList.Count
            });
        }
    }
}
