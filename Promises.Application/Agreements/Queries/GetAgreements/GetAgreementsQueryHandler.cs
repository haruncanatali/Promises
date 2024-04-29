using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Agreements.Queries.Dtos;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;

namespace Promises.Application.Agreements.Queries.GetAgreements;

public class GetAgreementsQueryHandler : IRequestHandler<GetAgreementsQuery, BaseResponseModel<GetAgreementsVm>>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAgreementsQueryHandler(IApplicationContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponseModel<GetAgreementsVm>> Handle(GetAgreementsQuery request, CancellationToken cancellationToken)
    {
        List<AgreementDto> agreements = await _context.Agreements
            .Where(c =>
                (request.Date == null || c.Date.Date == request.Date.Value.Date) &&
                (request.PersonId == null || c.PersonId == request.PersonId) &&
                (request.UserId == null || (c.UserId == _currentUserService.UserId)))
            .Include(c => c.Person)
            .Include(c => c.User)
            .Include(c => c.EventPhotos)
            .ProjectTo<AgreementDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return BaseResponseModel<GetAgreementsVm>.Success(new GetAgreementsVm
        {
            Agreements = agreements,
            Count = agreements.Count
        }, "Sözler başarıyla getirildi.");
    }
}