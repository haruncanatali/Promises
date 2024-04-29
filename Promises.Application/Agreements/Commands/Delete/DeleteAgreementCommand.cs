using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;

namespace Promises.Application.Agreements.Commands.Delete;

public class DeleteAgreementCommand : IRequest<BaseResponseModel<Unit>>
{
    public long Id { get; set; }
    
    public class Handler : IRequestHandler<DeleteAgreementCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseModel<Unit>> Handle(DeleteAgreementCommand request, CancellationToken cancellationToken)
        {
            Agreement? agreement = await _context.Agreements
                .Where(c=>c.Id == request.Id)
                .Include(c=>c.EventPhotos)
                .FirstOrDefaultAsync(cancellationToken);

            if (agreement == null)
                throw new BadRequestException("Silinecek söz bulunamadı.");

            if (agreement.EventPhotos.Count > 0)
            {
                _context.EventPhotos.RemoveRange(agreement.EventPhotos);
                await _context.SaveChangesAsync(cancellationToken);
            }

            _context.Agreements.Remove(agreement);
            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponseModel<Unit>.Success(Unit.Value, "Söz başarıyla silindi.");
        }
    }
}