using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;

namespace Promises.Application.EventPhotos.Commands.Delete;

public class DeleteEventPhotoCommand : IRequest<BaseResponseModel<Unit>>
{
    public long Id { get; set; }
    
    public class Handler : IRequestHandler<DeleteEventPhotoCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseModel<Unit>> Handle(DeleteEventPhotoCommand request, CancellationToken cancellationToken)
        {
            EventPhoto? eventPhoto = await _context.EventPhotos
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (eventPhoto == null)
                throw new BadRequestException("Silinecek söz fotoğrafı bulunamadı.");

            _context.EventPhotos.Remove(eventPhoto);
            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Söz fotoğrafı başarıyla silindi.");
        }
    }
}