using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Models;
using Promises.Application.Roles.Queries.Dtos;
using Promises.Domain.Identity;

namespace Promises.Application.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, BaseResponseModel<List<RoleDto>>>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;

    public GetRolesQueryHandler(RoleManager<Role> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<BaseResponseModel<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        List<RoleDto> roles = await _roleManager.Roles
            .Where(c => (request.Name == null || c.Name.ToLower().Contains(request.Name.ToLower())))
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return BaseResponseModel<List<RoleDto>>.Success(roles, "Success");
    }
}