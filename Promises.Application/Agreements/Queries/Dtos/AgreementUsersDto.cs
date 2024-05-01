using AutoMapper;
using Promises.Application.Common.Mappings;
using Promises.Domain.Entities;

namespace Promises.Application.Agreements.Queries.Dtos;

public class AgreementUsersDto : IMapFrom<AgreementUsers>
{
    public long PromiserUserId { get; set; } 
    public long PromisedUserId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AgreementUsers, AgreementDto>();
    }
}