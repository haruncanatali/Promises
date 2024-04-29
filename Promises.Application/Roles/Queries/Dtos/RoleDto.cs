using AutoMapper;
using Promises.Application.Common.Mappings;
using Promises.Domain.Identity;

namespace Promises.Application.Roles.Queries.Dtos;

public class RoleDto : IMapFrom<Role>
{
    public long Id { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Role, RoleDto>();
    }
}