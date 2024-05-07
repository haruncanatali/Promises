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

namespace Promises.Application.Friends.Commands.CreateBlock
{
    public class CreateBlockCommand : IRequest<BaseResponseModel<Unit>>
    {
        public long ReceiverId { get; set; }

        public class Handler : IRequestHandler<CreateBlockCommand, BaseResponseModel<Unit>>
        {
            private readonly IApplicationContext _context;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IApplicationContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<BaseResponseModel<Unit>> Handle(CreateBlockCommand request, CancellationToken cancellationToken)
            {
                User? user = await _context.Users
                    .FirstOrDefaultAsync(c => c.Id == request.ReceiverId, cancellationToken);

                if (user == null)
                    throw new BadRequestException("Engellenecek kullanıcı bulunamadı.");

                List<Agreement> agreements = await _context.Agreements
                    .Include(c => c.AgreementUsers)
                    .Include(c => c.EventPhotos)
                    .Where(c => c.AgreementUsers.First().PromisedUserId == request.ReceiverId)
                    .ToListAsync(cancellationToken);

                agreements.ForEach( async c =>
                {
                    _context.AgreementUsers.Remove(c.AgreementUsers.First());
                    _context.EventPhotos.RemoveRange(c.EventPhotos);
                    await _context.SaveChangesAsync(cancellationToken);
                    _context.Agreements.Remove(c);
                    await _context.SaveChangesAsync(cancellationToken);
                });

                await _context.BlockedFriends.AddAsync(new BlockedFriends
                {
                    SenderId = _currentUserService.UserId,
                    ReceiverId = request.ReceiverId
                }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return BaseResponseModel<Unit>.Success(Unit.Value, "Kişi başarıyla engellendi.");
            }
        }
    }
}
