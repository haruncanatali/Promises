using System.ComponentModel;

namespace Promises.Domain.Enums;

public enum PriorityLevel
{
    [Description("Düşük")]
    Low = 1,
    [Description("Orta")]
    Medium,
    [Description("Yüksek")]
    High,
    [Description("Çok Yüksek")]
    VeryHigh
}