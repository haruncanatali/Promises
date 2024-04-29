using System.ComponentModel;

namespace Promises.Domain.Enums;

public enum Gender
{
    [Description("Kadın")]
    Female = 1,
    [Description("Erkek")]
    Male,
}