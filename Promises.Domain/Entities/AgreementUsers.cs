using Promises.Domain.Base;

namespace Promises.Domain.Entities;

public class AgreementUsers : BaseEntity
{
    public long PromiserUserId { get; set; } // Söz veren
    public long PromisedUserId { get; set; } // Söz verilen
    public long AgreementId { get; set; }

    public Agreement Agreement { get; set; }
}