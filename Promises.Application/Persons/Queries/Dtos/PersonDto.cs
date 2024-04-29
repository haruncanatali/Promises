using AutoMapper;
using Promises.Application.Common.Mappings;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;

namespace Promises.Application.Persons.Queries.Dtos;

public class PersonDto : BaseDto,IMapFrom<Person>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Photo { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Person, PersonDto>();
    }
}