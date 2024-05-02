using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Domain.Identity;

public class User : IdentityUser<long>
{
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public Gender Gender { get; set; }
    public ZodiacSign ZodiacSign { get; set; }
    public string ProfilePhoto { get; set; }
    public DateTime Birthdate { get; set; }
    
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiredTime { get; set; }
    public string DeviceToken { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }

    public List<AgreementUsers> AgreementUsers { get; set; }
    public List<Agreement> Agreements { get; set; }

    [IgnoreDataMember]
    public string FullName
    {
        get { return $"{FirstName} {Surname}"; }
    }
}