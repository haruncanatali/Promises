using Promises.Domain.Base;

namespace Promises.Domain.Entities;

public class EventPhoto : BaseEntity
{
    public string Photo { get; set; }

    public long AgreementId { get; set; }

    public Agreement Agreement { get; set; }
}