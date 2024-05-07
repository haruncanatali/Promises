using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Identity;

namespace Promises.Application.Friends.Commands.DeleteBlock
{
    public class DeleteBlockCommand : IRequest<BaseResponseModel<Unit>>
    {
        public long ReceiverId { get; set; }

        public class Handler : IRequestHandler<DeleteBlockCommand , BaseResponseModel<Unit>>
        {
            private readonly IApplicationContext _context;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IApplicationContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<BaseResponseModel<Unit>> Handle(DeleteBlockCommand request, CancellationToken cancellationToken)
            {
                User? user = await _context.Users
                    .FirstOrDefaultAsync(c => c.Id == request.ReceiverId, cancellationToken);

                if (user == null)
                    throw new BadRequestException("Engeli kaldırılacak kişi bulunamadı.");

                BlockedFriends? blocked = await _context.BlockedFriends
                    .FirstOrDefaultAsync(
                        c => c.SenderId == _currentUserService.UserId && c.ReceiverId == request.ReceiverId,
                        cancellationToken);

                if (blocked == null)
                    throw new BadRequestException("Bu kişi engel listenizde bulunamadı.");

                _context.BlockedFriends.Remove(blocked);
                await _context.SaveChangesAsync(cancellationToken);

                return BaseResponseModel<Unit>.Success(Unit.Value, "Kişi engeli başarıyla kaldırıldı.");
            }
        }
    }
}
