using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Agreements.Queries.GetAgreements;

public class GetAgreementsQuery : IRequest<BaseResponseModel<GetAgreementsVm>>
{
    public string? Title { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? UserId { get; set; }
    public bool? Mine { get; set; }
}