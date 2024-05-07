using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Identity;

namespace Promises.Application.Friends.Commands.Delete;

public class DeleteFriendCommand : IRequest<BaseResponseModel<Unit>>
{
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
            User? user = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new BadRequestException("Silinecek arkadaş bulunamadı.");

            return null;
        }
    }
}