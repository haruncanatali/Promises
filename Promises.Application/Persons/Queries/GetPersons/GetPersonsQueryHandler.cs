using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Persons.Queries.Dtos;

namespace Promises.Application.Persons.Queries.GetPersons;

public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, BaseResponseModel<GetPersonsVm>>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetPersonsQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponseModel<GetPersonsVm>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        List<PersonDto> persons = await _context.Persons
            .Where(c =>
                (request.Name == null || c.Name.ToLower().Contains(request.Name.ToLower())) &&
                (request.Surname == null || c.Surname.ToLower().Contains(request.Surname.ToLower())))
            .ProjectTo<PersonDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return BaseResponseModel<GetPersonsVm>.Success(new GetPersonsVm
        {
            Persons = persons,
            Count = persons.Count
        }, "Kişiler başarıyla getirildi.");
    }
}