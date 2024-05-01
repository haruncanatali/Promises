using System.ComponentModel;

namespace Promises.Domain.Enums;

public enum NotificationFrequency
{
    [Description("Günde 1 Kere")]
    Day = 1,
    [Description("Haftada 1 Kere")]
    Week,
    [Description("Ayda 1 Kere")]
    Month,
    [Description("Yılda 1 Kere")]
    Year
}