using Promises.Domain.Base;

namespace Promises.Domain.Entities;

public class Friend : BaseEntity
{
    public long SenderId { get; set; }
    public long ReceiverId { get; set; }
    public bool Approved { get; set; }
}