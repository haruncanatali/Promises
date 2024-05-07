using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Enums;
using Promises.Domain.Identity;

namespace Promises.Application.Friends.Commands.Delete;

public class DeleteFriendCommand : IRequest<BaseResponseModel<Unit>>
{
    public long FriendId { get; set; }
    public long UserId { get; set; }
    
    public class Handler : IRequestHandler<DeleteFriendCommand,BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseModel<Unit>> Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
        {
            Friend? friend = await _context.Friends
                .FirstOrDefaultAsync(c => c.Id == request.FriendId, cancellationToken);

            if (friend == null)
                throw new BadRequestException("Arkadaşlık bulunamadı.");

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync(cancellationToken);

            List<Agreement> agreements = await _context.Agreements
                .Include(c => c.AgreementUsers)
                .Where(c => (c.EntityStatus == EntityStatus.Waiting) &&
                            (c.AgreementUsers.First().PromisedUserId == request.UserId &&
                             c.AgreementUsers.First().PromiserUserId == _currentUserService.UserId) &&
                            (c.AgreementUsers.First().PromiserUserId == request.UserId &&
                             c.AgreementUsers.First().PromisedUserId == _currentUserService.UserId))
                .ToListAsync(cancellationToken);

            agreements.ForEach(c =>
            {
                c.EntityStatus = EntityStatus.Failed;
            });

            _context.Agreements.UpdateRange(agreements);
            await _context.SaveChangesAsync(cancellationToken);


            return BaseResponseModel<Unit>.Success(Unit.Value, "Arkadaşlık başarıyla silindi.");
        }
    }
}