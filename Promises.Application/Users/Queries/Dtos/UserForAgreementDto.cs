using AutoMapper;
using Promises.Application.Common.Mappings;
using Promises.Domain.Identity;

namespace Promises.Application.Users.Queries.Dtos;

public class UserForAgreementDto : IMapFrom<User>
{
    public long Id { get; set; }
    public string FullName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserForAgreementDto>()
            .ForMember(dest => dest.FullName, opt =>
                opt.MapFrom(c => c.FullName));
    }
}