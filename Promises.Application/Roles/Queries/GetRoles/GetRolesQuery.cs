using MediatR;
using Promises.Application.Common.Models;
using Promises.Application.Roles.Queries.Dtos;

namespace Promises.Application.Roles.Queries.GetRoles;

public class GetRolesQuery : IRequest<BaseResponseModel<List<RoleDto>>>
{
    public string? Name { get; set; }
}