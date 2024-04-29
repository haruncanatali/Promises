using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.EventPhotos.Queries.GetEventPhoto;

public class GetEventPhotoQuery : IRequest<BaseResponseModel<GetEventPhotoVm>>
{
    public long Id { get; set; }
}