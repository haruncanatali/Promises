using MediatR;
using Microsoft.AspNetCore.Http;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.EventPhotos.Commands.Create;

public class CreateEventPhotoCommand : IRequest<BaseResponseModel<Unit>>
{
    public IFormFile Photo { get; set; }
    public long AgreementId { get; set; }
    
    public class Handler : IRequestHandler<CreateEventPhotoCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly FileManager _fileManager;

        public Handler(IApplicationContext context, FileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateEventPhotoCommand request, CancellationToken cancellationToken)
        {
            await _context.EventPhotos.AddAsync(new EventPhoto
            {
                Photo = _fileManager.Upload(request.Photo, FileRoot.EventPhotos),
                AgreementId = request.AgreementId
            });

            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Söz fotoğrafı başarıyla eklendi.");
        }
    }
}