using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Agreements.Queries.GetAgreement;

public class GetAgreementQuery : IRequest<BaseResponseModel<GetAgreementVm>>
{
    public long Id { get; set; }
}