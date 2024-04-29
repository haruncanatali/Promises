using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Persons.Queries.GetPersons;

public class GetPersonsQuery : IRequest<BaseResponseModel<GetPersonsVm>>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
}