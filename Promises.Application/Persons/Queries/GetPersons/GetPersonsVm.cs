using Promises.Application.Persons.Queries.Dtos;

namespace Promises.Application.Persons.Queries.GetPersons;

public class GetPersonsVm
{
    public List<PersonDto> Persons { get; set; }
    public long Count { get; set; }
}