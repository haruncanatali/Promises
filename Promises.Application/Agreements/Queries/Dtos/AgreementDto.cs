using AutoMapper;
using Promises.Application.Common.Helpers;
using Promises.Application.Common.Mappings;
using Promises.Application.Common.Models;
using Promises.Application.EventPhotos.Queries.Dtos;
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
    public string CommitmentStatusStr { get; set; }
    public bool HasNotification { get; set; }
    public string HasNotificactionStr { get; set; }
    public bool HasMailNotification { get; set; }
    public string HasMailNotificationStr { get; set; }
    public int NotificationFrequency { get; set; }

    public AgreementUsersDto AgreementUsers { get; set; }
    public UserForAgreementDto Promiser { get; set; }
    public UserForAgreementDto Promised { get; set; }
    public List<EventPhotoDto> EventPhotos { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Agreement, AgreementDto>()
            .ForMember(dest => dest.PriorityLevelStr, opt =>
                opt.MapFrom(c => c.PriorityLevel.GetDescription()))
            .ForMember(dest => dest.DateStr, opt =>
                opt.MapFrom(c => c.Date.ToString("dd/MM/yyyy HH:mm")))
            .ForMember(dest => dest.HasNotificactionStr, opt =>
                opt.MapFrom(c => c.HasNotification ? "Evet" : "Hayır"))
            .ForMember(dest => dest.HasMailNotificationStr, opt =>
                opt.MapFrom(c => c.HasMailNotification ? "Evet" : "Hayır"))
            .ForMember(dest => dest.EventPhotos, opt =>
                opt.MapFrom(c => c.EventPhotos))
            .ForMember(dest => dest.AgreementUsers, opt =>
                opt.MapFrom(c=>c.AgreementUsers));
    }
}