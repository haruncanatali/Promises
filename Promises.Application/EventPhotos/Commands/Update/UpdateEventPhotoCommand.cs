using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.EventPhotos.Commands.Update;

public class UpdateEventPhotoCommand : IRequest<BaseResponseModel<Unit>>
{
    public long Id { get; set; }
    public IFormFile Photo { get; set; }
    public long AgreementId { get; set; }
    
    public class Handler : IRequestHandler<UpdateEventPhotoCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly FileManager _fileManager;

        public Handler(IApplicationContext context, FileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<BaseResponseModel<Unit>> Handle(UpdateEventPhotoCommand request, CancellationToken cancellationToken)
        {
            EventPhoto? eventPhoto = await _context.EventPhotos
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (eventPhoto == null)
                throw new BadRequestException("Güncellenecek sçz fotoğrafı bulunamadı.");

            eventPhoto.AgreementId = request.AgreementId;
            eventPhoto.Photo = _fileManager.Upload(request.Photo, FileRoot.EventPhotos);

            _context.EventPhotos.Update(eventPhoto);
            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value,"Söz fotoğrafı başarıyla güncellendi.");
        }
    }
}