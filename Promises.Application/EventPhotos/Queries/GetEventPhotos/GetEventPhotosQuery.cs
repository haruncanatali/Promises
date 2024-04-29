using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.EventPhotos.Queries.GetEventPhotos;

public class GetEventPhotosQuery : IRequest<BaseResponseModel<GetEventPhotosVm>>
{
    public long? AgreementId { get; set; }
}