using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Persons.Queries.Dtos;

namespace Promises.Application.Persons.Queries.GetPerson;

public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, BaseResponseModel<GetPersonVm>>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetPersonQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponseModel<GetPersonVm>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        PersonDto? person = await _context.Persons
            .Where(c => c.Id == request.Id)
            .ProjectTo<PersonDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (person == null)
            throw new BadRequestException("Kişi bulunamadı.");
        
        return BaseResponseModel<GetPersonVm>.Success(new GetPersonVm
        {
            Person = person
        }, "Kişi başarıyla getirildi.");
    }
}