using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Application.EventPhotos.Queries.Dtos;

namespace Promises.Application.EventPhotos.Queries.GetEventPhotos;

public class GetEventPhotosQueryHandler : IRequestHandler<GetEventPhotosQuery, BaseResponseModel<GetEventPhotosVm>>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    
    public GetEventPhotosQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponseModel<GetEventPhotosVm>> Handle(GetEventPhotosQuery request, CancellationToken cancellationToken)
    {
        List<EventPhotoDto> eventPhotos = await _context.EventPhotos
            .Where(c => (request.AgreementId == null || c.AgreementId == request.AgreementId))
            .ProjectTo<EventPhotoDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return BaseResponseModel<GetEventPhotosVm>.Success(new GetEventPhotosVm
        {
            EventPhotos = eventPhotos,
            Count = eventPhotos.Count
        }, "Söz fotoğrafları başarıyla getirildi.");
    }
}