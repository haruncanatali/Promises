using AutoMapper;
using Promises.Application.Common.Mappings;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;

namespace Promises.Application.EventPhotos.Queries.Dtos;

public class EventPhotoDto : BaseDto, IMapFrom<EventPhoto>
{
    public long Id { get; set; }
    public string Photo { get; set; }
    public long AgreementId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EventPhoto, EventPhotoDto>();
    }
}