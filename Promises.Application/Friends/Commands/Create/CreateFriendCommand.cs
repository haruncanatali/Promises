using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Notifications.Commands.FriendRequestNotification;
using Promises.Domain.Entities;
using Promises.Domain.Identity;

namespace Promises.Application.Friends.Commands.Create;

public class CreateFriendCommand : IRequest<BaseResponseModel<Unit>>
{
    public long ReceiverId { get; set; }
    
    public class Handler : IRequestHandler<CreateFriendCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;

        public Handler(IApplicationContext context, ICurrentUserService currentUserService, IMediator mediator)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateFriendCommand request, CancellationToken cancellationToken)
        {
            User? receiver = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == request.ReceiverId, cancellationToken);

            if (receiver == null)
                throw new BadRequestException("Eklenecek arkadaş sistemde bulunamadı.");

            var blockedIds = await _context.BlockedFriends
                .Where(c => (c.ReceiverId == _currentUserService.UserId || c.SenderId == _currentUserService.UserId))
                .Select(c => c.ReceiverId == _currentUserService.UserId ? c.SenderId : c.ReceiverId)
                .ToListAsync(cancellationToken);

            if (blockedIds.Contains(request.ReceiverId))
                throw new BadRequestException("Bu kişi ile arkadaş olmanız engellenmiştir.");

            await _context.Friends.AddAsync(new Friend
            {
                SenderId = _currentUserService.UserId,
                ReceiverId = request.ReceiverId,
                Approved = false
            });

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new FriendRequestNotificationCommand
            {
                ReceiverDeviceId = receiver.DeviceToken,
                SenderFullName = _currentUserService.FullName
            });
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirim başarıyla gönderildi.");
        }
    }
}