using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.EventPhotos.Queries.Dtos;
using Promises.Domain.Entities;

namespace Promises.Application.EventPhotos.Queries.GetEventPhoto;

public class GetEventPhotoQueryHandler : IRequestHandler<GetEventPhotoQuery, BaseResponseModel<GetEventPhotoVm>>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetEventPhotoQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponseModel<GetEventPhotoVm>> Handle(GetEventPhotoQuery request, CancellationToken cancellationToken)
    {
        EventPhotoDto? eventPhoto = await _context
            .EventPhotos
            .Where(c => c.Id == request.Id)
            .ProjectTo<EventPhotoDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (eventPhoto == null)
            throw new BadRequestException("Söz fotoğrafı bulunamadı.");
        
        return BaseResponseModel<GetEventPhotoVm>.Success(new GetEventPhotoVm
        {
            EventPhoto = eventPhoto
        }, "Söz fotoğrafı başarıyla getirildi.");
    }
}