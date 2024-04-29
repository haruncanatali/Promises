using Promises.Application.EventPhotos.Queries.Dtos;

namespace Promises.Application.EventPhotos.Queries.GetEventPhotos;

public class GetEventPhotosVm
{
    public List<EventPhotoDto> EventPhotos { get; set; }
    public long Count { get; set; }
}