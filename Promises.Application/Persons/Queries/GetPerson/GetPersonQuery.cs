using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Persons.Queries.GetPerson;

public class GetPersonQuery : IRequest<BaseResponseModel<GetPersonVm>>
{
    public long Id { get; set; }
}