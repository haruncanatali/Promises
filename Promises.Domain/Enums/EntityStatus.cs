using System.ComponentModel;

namespace Promises.Domain.Enums;

public enum EntityStatus
{
    [Description("Bekliyor")]
    Waiting = 1,
    [Description("Başarısız")]
    Failed,
    [Description("Başarılı")]
    Success
}