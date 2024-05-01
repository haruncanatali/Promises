using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Agreements.Queries.Dtos;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Users.Queries.Dtos;

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
            .Include(c=>c.AgreementUsers)
            .Include(c=>c.EventPhotos)
            .Where(c =>
                (request.Date == null || c.Date.Date == request.Date.Value.Date) &&
                (request.StartDate == null || c.Date.Date >= request.StartDate.Value.Date) &&
                (request.EndDate == null || c.Date.Date <= request.EndDate.Value.Date) &&
                (request.Mine == null || (request.Mine == true && (c.AgreementUsers.First().PromisedUserId == _currentUserService.UserId || c.AgreementUsers.First().PromiserUserId == _currentUserService.UserId))) &&
                (request.UserId == null || (request.UserId != null && (c.AgreementUsers.First().PromisedUserId == request.UserId || c.AgreementUsers.First().PromiserUserId == request.UserId)))
            )
            .ProjectTo<AgreementDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        agreements.ForEach(async c =>
        {
            c.Promiser = await _context.Users.Where(x => x.Id == c.AgreementUsers.PromiserUserId)
                .ProjectTo<UserForAgreementDto>(_mapper.ConfigurationProvider)
                .FirstAsync(cancellationToken);
            c.Promised = await _context.Users.Where(x => x.Id == c.AgreementUsers.PromisedUserId)
                .ProjectTo<UserForAgreementDto>(_mapper.ConfigurationProvider)
                .FirstAsync(cancellationToken);
        });
        
        return BaseResponseModel<GetAgreementsVm>.Success(new GetAgreementsVm
        {
            Agreements = agreements,
            Count = agreements.Count
        }, "Sözler başarıyla getirildi.");
    }
}