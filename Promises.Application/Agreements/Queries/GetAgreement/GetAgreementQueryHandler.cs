using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Agreements.Queries.Dtos;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;

namespace Promises.Application.Agreements.Queries.GetAgreement;

public class GetAgreementQueryHandler : IRequestHandler<GetAgreementQuery, BaseResponseModel<GetAgreementVm>>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetAgreementQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponseModel<GetAgreementVm>> Handle(GetAgreementQuery request, CancellationToken cancellationToken)
    {
        AgreementDto? agreement = await _context.Agreements
            .Where(c => c.Id == request.Id)
            .Include(c => c.Person)
            .Include(c => c.User)
            .Include(c=>c.EventPhotos)
            .ProjectTo<AgreementDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (agreement == null)
            throw new BadRequestException("Söz bulunamadı.");
        
        return BaseResponseModel<GetAgreementVm>.Success(new GetAgreementVm
        {
            Agreement = agreement
        }, "Söz başarıyla getirildi.");
    }
}