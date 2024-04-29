using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Agreements.Queries.GetAgreements;

public class GetAgreementsQuery : IRequest<BaseResponseModel<GetAgreementsVm>>
{
    public DateTime? Date { get; set; }
    public long? PersonId { get; set; }
    public long? UserId { get; set; }
}