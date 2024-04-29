using AutoMapper;
using Promises.Application.Common.Helpers;
using Promises.Application.Common.Mappings;
using Promises.Application.Common.Models;
using Promises.Application.EventPhotos.Queries.Dtos;
using Promises.Application.Persons.Queries.Dtos;
using Promises.Application.Users.Queries.Dtos;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.Agreements.Queries.Dtos;

public class AgreementDto: BaseDto, IMapFrom<Agreement>
{
    public string Description { get; set; }
    public PriorityLevel PriorityLevel { get; set; }
    public string PriorityLevelStr { get; set; }
    public DateTime Date { get; set; }
    public string DateStr { get; set; }
    public CommitmentStatus CommitmentStatus { get; set; }
    public string CommitmentStatusStr { get; set; }
    public bool HasNotification { get; set; }
    public string HasNotificactionStr { get; set; }
    public bool HasMailNotification { get; set; }
    public string HasMailNotificationStr { get; set; }
    public int NotificationFrequency { get; set; }
    public long PersonId { get; set; }
    public long UserId { get; set; }

    public PersonDto Person { get; set; }
    public UserForAgreementDto User { get; set; }
    public List<EventPhotoDto> EventPhotos { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Agreement, AgreementDto>()
            .ForMember(dest => dest.PriorityLevelStr, opt =>
                opt.MapFrom(c => c.PriorityLevel.GetDescription()))
            .ForMember(dest => dest.DateStr, opt =>
                opt.MapFrom(c => c.Date.ToString("dd/MM/yyyy HH:mm")))
            .ForMember(dest => dest.CommitmentStatusStr, opt =>
                opt.MapFrom(c => c.CommitmentStatus.GetDescription()))
            .ForMember(dest => dest.HasNotificactionStr, opt =>
                opt.MapFrom(c => c.HasNotification ? "Evet" : "Hayır"))
            .ForMember(dest => dest.HasMailNotificationStr, opt =>
                opt.MapFrom(c => c.HasMailNotification ? "Evet" : "Hayır"))
            .ForMember(dest => dest.Person, opt =>
                opt.MapFrom(c => c.Person))
            .ForMember(dest => dest.User, opt =>
                opt.MapFrom(c => c.User))
            .ForMember(dest => dest.EventPhotos, opt =>
                opt.MapFrom(c => c.EventPhotos));
    }
}