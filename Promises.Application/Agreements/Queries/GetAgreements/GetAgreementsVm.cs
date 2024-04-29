using Promises.Application.Agreements.Queries.Dtos;

namespace Promises.Application.Agreements.Queries.GetAgreements;

public class GetAgreementsVm
{
    public List<AgreementDto> Agreements { get; set; }
    public long Count { get; set; }
}