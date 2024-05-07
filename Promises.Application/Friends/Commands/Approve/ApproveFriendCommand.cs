using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Notifications.Commands.FriendRequestNotification;
using Promises.Application.Notifications.Commands.FriendResponseNotification;
using Promises.Domain.Entities;
using Promises.Domain.Identity;

namespace Promises.Application.Friends.Commands.Approve;

public class ApproveFriendCommand : IRequest<BaseResponseModel<Unit>>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public bool Approve { get; set; }
    
    public class Handler : IRequestHandler<ApproveFriendCommand, BaseResponseModel<Unit>>
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

        public async Task<BaseResponseModel<Unit>> Handle(ApproveFriendCommand request, CancellationToken cancellationToken)
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new BadRequestException("İsteğinizi işleyebileceğiniz kişi bulunamadı.");

            Friend? friend = await _context.Friends
                .FirstOrDefaultAsync(c => c.Id == request.FriendId, cancellationToken);

            if (friend == null)
                throw new BadRequestException("İsteğinizi işleyebileceğiniz bir arkadaşlık isteği bulunamadı.");

            friend.Approved = request.Approve;
            _context.Friends.Update(friend);
            await _context.SaveChangesAsync(cancellationToken);
            
            await _mediator.Send(new FriendResponseNotificationCommand
            {
                ReceiverDeviceId = user.DeviceToken,
                SenderFullName = _currentUserService.FullName,
                Approved = request.Approve
            });
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Arkadaşlık durumu başarıyla işlendi.");
        }
    }
}