using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Friends.Queries.Dtos;

namespace Promises.Application.Friends.Queries.GetFriends
{
    public class GetFriendsQueryHandler : IRequestHandler<GetFriendsQuery, BaseResponseModel<GetFriendsVm>>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetFriendsQueryHandler(IApplicationContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseModel<GetFriendsVm>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
        {
            List<FriendDto> friends = new List<FriendDto>();

            var partialUsers = await _context.Friends
                .Where(c =>
                    (c.SenderId == _currentUserService.UserId || c.ReceiverId == _currentUserService.UserId) &&
                    (c.Approved == request.Approved))
                .Select(c => new
                {
                    UserId = c.ReceiverId == _currentUserService.UserId ? c.SenderId : c.ReceiverId,
                    FriendshipDuration = c.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var users = await _context.Users
                .Where(c=>partialUsers.Select(x=>x.UserId).Contains(c.Id) && 
                          (request.FullName == null || c.FullName.ToLower().Contains(request.FullName.ToLower())))
                .Select(c => new
                {
                    UserId = c.Id,
                    UserFullName = c.FullName,
                    FriendshipDuration = partialUsers.Where(m => m.UserId == c.Id).Select(m=>m.FriendshipDuration).FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            users.ForEach(c =>
            {
                friends.Add(new FriendDto
                {
                    UserId = c.UserId,
                    UserFullName = c.UserFullName,
                    FriendshipDuration = c.FriendshipDuration
                });
            });


            return BaseResponseModel<GetFriendsVm>.Success(new GetFriendsVm
            {
                Friends = friends,
                Count = friends.Count
            }, "Arkadaşların başarıyla getirildi.");
        }
    }
}
