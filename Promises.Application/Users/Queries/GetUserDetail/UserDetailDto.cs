using AutoMapper;
using Promises.Application.Common.Helpers;
using Promises.Application.Common.Mappings;
using Promises.Domain.Enums;
using Promises.Domain.Identity;

namespace Promises.Application.Users.Queries.GetUserDetail
{
    public class UserDetailDto : IMapFrom<User>
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public ZodiacSign ZodiacSign { get; set; }
        public string GenderText { get; set; }
        public string ZodiacSignText { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime Birthdate { get; set; }
        public string BirthdateText { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDetailDto>()
                .ForMember(dest => GenderText, opt => 
                    opt.MapFrom(c=>c.Gender.GetDescription()))
                .ForMember(dest => dest.ZodiacSignText, opt =>
                    opt.MapFrom(c=>c.ZodiacSign.GetDescription()))
                .ForMember(dest => dest.BirthdateText, opt =>
                    opt.MapFrom(c=>c.Birthdate.ToString("dd/MM/yyyy")));
        }
    }
}